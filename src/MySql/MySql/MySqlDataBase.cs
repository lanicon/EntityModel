// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // 作者:
// // 工程:Agebull.DataModel
// // 建立:2016-06-07
// // 修改:2016-06-16
// // *****************************************************/

#region 引用

using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using Agebull.Common.Logging;
using System.Data.Common;
using System.Threading.Tasks;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;

#endregion

namespace Agebull.EntityModel.MySql
{
    /// <summary>
    ///     表示SQL SERVER数据库对象
    /// </summary>
    public abstract partial class MySqlDataBase : MySqlDataBase_, IDataBase
    {
        #region 事务

        //protected MySqlDataBase()
        //{
        //    Trace.WriteLine(".ctor", "MySqlDataBase");
        //}

        /// <summary>
        ///     事务对象
        /// </summary>
        public MySqlTransaction Transaction { get; internal set; }

        /// <inheritdoc />
        /// <summary>
        ///     事务对象
        /// </summary>
        DbTransaction IDataBase.Transaction => Transaction;

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public bool BeginTransaction()
        {
            if (Transaction != null)
                return false;
            Transaction = Connection.BeginTransaction();

            return true;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        void IDataBase.Rollback()
        {
            Transaction?.Rollback();
            Transaction?.Dispose();
            Transaction = null;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        void IDataBase.Commit()
        {
            Transaction?.Commit();
            Transaction?.Dispose();
            Transaction = null;
        }
        #endregion

        #region 引用范围

        /// <summary>
        /// 生成数据库使用范围
        /// </summary>
        /// <returns></returns>
        IDisposable IDataBase.CreateDataBaseScope() => DataBaseScope.CreateScope(this);

        /// <summary>
        ///     引用数量
        /// </summary>
        public int QuoteCount { get; set; }

        #endregion

        #region 数据库连接对象

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataBaseType => DataBaseType.MySql;

        private MySqlConnection _connection;

        /// <summary>
        ///     连接对象
        /// </summary>
        public MySqlConnection Connection => _connection ??= InitConnection();

        /// <summary>
        ///     连接对象
        /// </summary>
        public static readonly List<MySqlConnection> Connections = new List<MySqlConnection>();

        /*// <summary>
        ///     连接对象
        /// </summary>
        public static readonly Queue<MySqlConnection> IdleConnections = new Queue<MySqlConnection>();*/

        /// <summary>
        /// 初始化连接对象
        /// </summary>
        /// <returns></returns>
        private MySqlConnection InitConnection()
        {
            //MySqlConnection connection;
            //lock (IdleConnections)
            //{
            //    IdleConnections.TryDequeue(out connection);
            //}

            //if (connection != null)
            //{
            //    if (connection.State == ConnectionState.Open)
            //        return connection;
            //    lock (Connections)
            //    {
            //        Connections.Remove(connection);
            //    }
            //    connection.Dispose();
            //}

            var connection = new MySqlConnection(ConnectionString);
            IocScope.DisposeFunc.Add(() => Close(connection));
            lock (Connections)
            {
                Connections.Add(connection);
            }
            connection.Open();
            lock (Connections)
                LogRecorderX.Trace($"打开连接数：{Connections.Count}");
            return connection;
        }

        #endregion

        #region 连接

        /// <summary>
        ///     连接字符串
        ///     Database=test;Data Source=localhost;User Id=root;Password=123456;pooling=false;CharSet=utf8;port=3306
        /// </summary>
        private string _connectionString;

        /// <summary>
        ///     连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (_connectionString != null)
                {
                    return _connectionString;
                }

                var str = LoadConnectionStringSetting();
                var b = new MySqlConnectionStringBuilder(str);
                //if (b.ConnectionTimeout <= 0 || b.ConnectionTimeout > 10)
                //    b.ConnectionTimeout = 10;

                //if (b.DefaultCommandTimeout <= 0 || b.DefaultCommandTimeout > 10)
                //    b.DefaultCommandTimeout = 10;

                DataBaseName = b.Database;
                return _connectionString = b.ConnectionString;
            }
        }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBaseName { get; private set; }

        /// <summary>
        /// 读取连接字符串
        /// </summary>
        /// <returns></returns>
        protected abstract string LoadConnectionStringSetting();

        /// <summary>
        ///     打开连接
        /// </summary>
        /// <returns>是否打开,是则为此时打开,否则为之前已打开</returns>
        public bool Open()
        {
            if (_connection != null/* && _connection.State == ConnectionState.Open*/)
                return false;
            _connection = InitConnection();
            return true;
        }

        /// <summary>
        ///     关闭连接
        /// </summary>
        void IDataBase.Free()
        {
            if (_connection == null)
                return;
            //if (_connection.State == ConnectionState.Open)
            //    lock (IdleConnections)
            //    {
            //        IdleConnections.Enqueue(_connection);
            //    }
            //else
                Close(_connection);
            _connection = null;
        }

        /// <summary>
        ///     关闭连接
        /// </summary>
        public void Close()
        {
            Close(_connection);
            _connection = null;
        }

        /// <summary>
        ///     关闭连接
        /// </summary>
        private void Close(MySqlConnection connection)
        {
            //int cnt;
            lock (Connections)
            {
                if (!Connections.Remove(connection))
                    return;
                //cnt = Connections.Count;
            }
            if (connection == null)
            {
                return;
            }
            if (connection.State == ConnectionState.Open)
            {
                try
                {
                    connection.Close();
                }
                catch (Exception exception)
                {
                    LogRecorderX.Exception(exception);
                }
            }

            try
            {
                connection.Dispose();
            }
            catch (Exception exception)
            {
                LogRecorderX.Exception(exception);
            }
            //lock (Connections)
            //    LogRecorderX.Trace($"未关闭总数：{Connections.Count}");
        }

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        protected virtual void DoDispose()
        {
        }

        private bool _isDisposed;

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            DoDispose();
            Close();
        }

        /// <summary>
        /// 析构
        /// </summary>
        ~MySqlDataBase()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            DoDispose();
            Close();
        }
        #endregion

        #region 数据库特殊操作

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public int Execute(string sql)
        {
            //using (DataBaseScope.CreateScope(this))
            {
                return ExecuteInner(sql);
            }
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public int Execute(string sql, IEnumerable<DbParameter> args)
        {
            return ExecuteInner(sql, args.ToArray());
        }


        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public int Execute(string sql, params DbParameter[] args)
        {
            return args.Length == 0
                ? ExecuteInner(sql)
                : ExecuteInner(sql, args);
        }

        /// <summary>
        ///     执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public object ExecuteScalar(string sql, IEnumerable<DbParameter> args)
        {
            //using (DataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner(sql, args.ToArray());
            }
        }

        /// <summary>
        ///     执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public object ExecuteScalar(string sql, params DbParameter[] args)
        {
            //using (DataBaseScope.CreateScope(this))
            {
                return args == null || args.Length == 0
                    ? ExecuteScalarInner(sql)
                    : ExecuteScalarInner(sql, args);
            }
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public T ExecuteScalar<T>(string sql)
        {
            //using (DataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner<T>(sql);
            }
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public object ExecuteScalar(string sql)
        {
            //using (DataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner(sql);
            }
        }


        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        public T ExecuteScalar<T>(string sql, params DbParameter[] args)
        {
            //using (DataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner<T>(sql, args);
            }
        }


        /// <summary>
        ///     清除所有数据
        /// </summary>
        public void Clear(string table)
        {
            Execute($@"TRUNCATE TABLE `{table}`;");
        }

        /// <summary>
        ///     清除所有数据
        /// </summary>
        public void ClearAll()
        {
            var sql = new StringBuilder();
            foreach (var table in TableSql.Values)
            {
                Clear(table.TableName);
            }

            Execute(sql.ToString());
        }

        #endregion

        #region 内部方法

        /// <summary>
        ///     对连接执行 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>被影响的行数</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?序号的形式访问参数
        /// </remarks>
        protected int ExecuteInner(string sql, params DbParameter[] args)
        {
            using var cmd = CreateCommand(sql, args);
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        ///     执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        protected object ExecuteScalarInner(string sql, params DbParameter[] args)
        {
            object result;
            using (var cmd = CreateCommand(sql, args))
            {
                result = cmd.ExecuteScalar();
            }
            return result == DBNull.Value ? null : result;
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        protected T ExecuteScalarInner<T>(string sql)
        {
            return (T)ExecuteScalarInner(sql);
        }

        /// <summary>
        ///     执行SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        protected T ExecuteScalarInner<T>(string sql, params DbParameter[] args)
        {
            var result = args.Length == 0
                ? ExecuteScalarInner(sql)
                : ExecuteScalarInner(sql, args);
            return (T)result;
        }

        /// <summary>
        ///     记录SQL日志
        /// </summary>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        public static void TraceSql(MySqlCommand cmd)
        {
            if (!LogRecorderX.LogDataSql)
                return;
            TraceSql(cmd.CommandText, cmd.Parameters.Cast<MySqlParameter>());
        }

        /// <summary>
        ///     记录SQL日志
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="args">参数</param>
        /// <returns>操作的第一行第一列或空</returns>
        /// <remarks>
        ///     注意,如果有参数时,都是匿名参数,请使用?的形式访问参数
        /// </remarks>
        public static void TraceSql(string sql, IEnumerable<MySqlParameter> args)
        {
            if (!LogRecorderX.LogDataSql)
                return;
            StringBuilder code = new StringBuilder();
            code.AppendLine("/***************************************************************/");
            var parameters = args as MySqlParameter[] ?? args.ToArray();
            foreach (var par in parameters)
            {
                code.AppendLine($"declare ?{par.ParameterName} {par.MySqlDbType};");
            }
            foreach (var par in parameters.Where(p => p.Value != null && !p.IsNullable))
            {
                code.AppendLine($"SET ?{par.ParameterName} = '{par.Value}';");
            }
            foreach (var par in parameters.Where(p => p.Value == null || p.IsNullable))
            {
                code.AppendLine($"SET ?{par.ParameterName} = NULL;");
            }
            code.AppendLine(sql);
            LogRecorderX.RecordDataLog(code.ToString());
        }


        #endregion

        #region 生成命令对象

        /// <summary>
        ///     生成命令
        /// </summary>
        public MySqlCommand CreateCommand(params DbParameter[] args)
        {
            return CreateCommand(null, args);
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        public MySqlCommand CreateCommand(string sql, DbParameter arg)
        {
            return CreateCommand(sql, new[] { arg });
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        public async Task<MySqlCommand> CreateCommandAsync()
        {
            if (_connection == null)
                await OpenAsync();
            var cmd = Connection.CreateCommand();

            if (Transaction != null)
            {
                cmd.Transaction = Transaction;
            }
            return cmd;
        }
        /// <summary>
        ///     生成命令
        /// </summary>
        public async Task<MySqlCommand> CreateCommandAsync(string sql, IEnumerable<DbParameter> args = null)
        {
            if (_connection == null)
                await OpenAsync();
            var cmd = Connection.CreateCommand();

            if (Transaction != null)
            {
                cmd.Transaction = Transaction;
            }
            if (sql != null)
            {
                cmd.CommandText = sql;
            }
            if (args != null)
            {
                var parameters = args.OfType<MySqlParameter>().ToArray();
                if (parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
            }
            TraceSql(cmd);
            return cmd;
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        public MySqlCommand CreateCommand(string sql, IEnumerable<DbParameter> args = null)
        {
            var cmd = Connection.CreateCommand();

            if (Transaction != null)
            {
                cmd.Transaction = Transaction;
            }
            if (sql != null)
            {
                cmd.CommandText = sql;
            }
            if (args != null)
            {
                var parameters = args.OfType<MySqlParameter>().ToArray();
                if (parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
            }
            TraceSql(cmd);
            return cmd;
        }

        #endregion

        #region 创建表的SQL字典

        /// <summary>
        ///     表的常用SQL
        /// </summary>
        protected Dictionary<string, TableSql> _tableSql;

        /// <summary>
        ///     表的常用SQL
        /// </summary>
        /// <remarks>请设置为键大小写不敏感字典,因为Sql没有强制表名的大小写区别</remarks>
        public Dictionary<string, TableSql> TableSql => _tableSql ??= new Dictionary<string, TableSql>(StringComparer.OrdinalIgnoreCase);

        #endregion


        #region 数据缓存

        /*// <summary>
        ///     缓存数据
        /// </summary>
        private readonly Dictionary<int, Dictionary<long, EditDataObject>> _dataCache =
            new Dictionary<int, Dictionary<long, EditDataObject>>();

        /// <summary>
        ///     取缓存数据
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="table"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public TData GetData<TData>(int table, int id) where TData : EditDataObject
        {
            Dictionary<long, EditDataObject> tableDatas;
            if (!_dataCache.TryGetValue(table, out tableDatas))
            {
                return null;
            }
            EditDataObject data;
            if (!tableDatas.TryGetValue(id, out data))
            {
                return null;
            }
            return data as TData;
        }

        /// <summary>
        ///     如不存在于缓存中，则加入，返回自身，如存在，则返回缓存中的数据。
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="table"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public TData TryAddToCache<TData>(int table, long id, TData data) where TData : EditDataObject
        {
            Dictionary<long, EditDataObject> tableDatas;
            if (!_dataCache.TryGetValue(table, out tableDatas))
            {
                _dataCache.Add(table, tableDatas = new Dictionary<long, EditDataObject>());
            }

            if (tableDatas.ContainsKey(id))
            {
                return tableDatas[id] as TData;
            }
            tableDatas.Add(id, data);
            return data;
        }*/


        #endregion


        #region 接口

        string IDataBase.ConnectionString => ConnectionString;


        int IDataBase.Execute(string sql, IEnumerable<DbParameter> args)
        {
            return Execute(sql, args);
        }

        int IDataBase.Execute(string sql, params DbParameter[] args)
        {
            return Execute(sql, args);
        }

        object IDataBase.ExecuteScalar(string sql, IEnumerable<DbParameter> args)
        {
            return ExecuteScalar(sql, args);
        }

        object IDataBase.ExecuteScalar(string sql, params DbParameter[] args)
        {
            return ExecuteScalar(sql, args.ToArray());
        }

        T IDataBase.ExecuteScalar<T>(string sql)
        {
            return ExecuteScalar<T>(sql);
        }

        object IDataBase.ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql);
        }

        T IDataBase.ExecuteScalar<T>(string sql, params DbParameter[] args)
        {
            return ExecuteScalar<T>(sql, args.ToArray());
        }

        DbCommand IDataBase.CreateCommand(params DbParameter[] args)
        {
            return CreateCommand(args.ToArray());
        }

        DbCommand IDataBase.CreateCommand(string sql, DbParameter arg)
        {
            return CreateCommand(arg);
        }

        DbCommand IDataBase.CreateCommand(string sql, IEnumerable<DbParameter> args)
        {
            return CreateCommand(sql, args.ToArray());
        }



        #endregion
    }

}