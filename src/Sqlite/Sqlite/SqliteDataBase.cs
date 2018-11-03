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
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Agebull.Common.Logging;
using System.Data.Common;
using Agebull.Common.Ioc;

#endregion

namespace Agebull.Common.DataModel.Sqlite
{
    /// <summary>
    ///     表示SQLite数据库对象
    /// </summary>
    public abstract class SqLiteDataBase : SimpleConfig, IDataBase
    {
        #region 事务

        //protected SqliteDataBase()
        //{
        //    Trace.WriteLine(".ctor", "SqliteDataBase");
        //}

        /// <summary>
        ///     事务
        /// </summary>
        public SQLiteTransaction Transaction { get; internal set; }

        #endregion

        #region 引用范围

        /// <summary>
        /// 生成数据库使用范围
        /// </summary>
        /// <returns></returns>
        IDisposable IDataBase.CreateDataBaseScope() => SqliteDataBaseScope.CreateScope(this);


        /// <summary>
        /// 生成事务范围
        /// </summary>
        /// <returns></returns>
        public ITransactionScope CreateTransactionScope() => TransactionScope.CreateScope(this);

        /// <summary>
        ///     引用数量
        /// </summary>
        protected internal int QuoteCount { get; set; }

        #endregion

        #region 线程实例

        /// <summary>
        /// 构造
        /// </summary>
        protected SqLiteDataBase()
        {
            if (_default == null)
                _default = this;
        }

        /// <summary>
        ///     锁对象
        /// </summary>
        protected static readonly object LockData = new object();

        /// <summary>
        ///     缺省强类型数据库
        /// </summary>
        [ThreadStatic]
        private static SqLiteDataBase _default;

        /// <summary>
        ///     连接对象
        /// </summary>
        public static SqLiteDataBase DataBase
        {
            get => _default ?? (_default = IocHelper.Create<SqLiteDataBase>());
            set => _default = value;
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
        public string ConnectionString => _connectionString ?? (_connectionString = LoadConnectionStringSetting());

        /// <summary>
        /// 读取连接字符串
        /// </summary>
        /// <returns></returns>
        protected abstract string LoadConnectionStringSetting();


        SQLiteConnection _connection;
        /// <summary>
        ///     连接对象
        /// </summary>
        public SQLiteConnection Connection => _connection ?? (_connection = InitConnection());

        private static readonly List<SQLiteConnection> Connections = new List<SQLiteConnection>();
        /// <summary>
        ///     打开连接
        /// </summary>
        /// <returns>是否打开,是则为此时打开,否则为之前已打开</returns>
        public bool Open()
        {
            lock (LockData)
            {
                //if (_isClosed)
                //{
                //    //throw new Exception("已关闭的数据库对象不能再次使用");
                //}
                bool result = false;
                if (_connection == null)
                {
                    result = true;
                    _connection = new SQLiteConnection(ConnectionString);
                    Connections.Add(_connection);
                    //Trace.WriteLine("Create _connection", "SqliteDataBase");
                }
                else if (string.IsNullOrEmpty(_connection.ConnectionString))
                {
                    result = true;
                    //Trace.WriteLine("Set ConnectionString", "SqliteDataBase");
                    _connection.ConnectionString = ConnectionString;
                }
                if (_connection.State == ConnectionState.Open)
                {
                    return result;
                }
                //Trace.WriteLine(_count++, "Open");
                //Trace.WriteLine("Opened _connection", "SqliteDataBase");
                _connection.Open();
            }
            return true;
        }

        private SQLiteConnection InitConnection()
        {
            lock (LockData)
            {
                var connection = new SQLiteConnection(ConnectionString);
                Connections.Add(connection);
                //Trace.WriteLine(_count++, "Open");
                //Trace.WriteLine("Opened _connection", "SqliteDataBase");
                connection.Open();
                return connection;
            }
        }
        /// <summary>
        ///     关闭连接
        /// </summary>
        public void Close()
        {
            if (_connection == null)
            {
                return;
            }
            lock (LockData)
            {
                try
                {
                    if (_connection.State == ConnectionState.Open)
                    {
                        //Trace.WriteLine("Close Connection", "SqliteDataBase");
                        _connection.Close();
                    }
                    Connections.Remove(_connection);
                    LogRecorder.MonitorTrace($"未关闭总数{Connections.Count}");
                    _connection.Dispose();
                    _connection = null;

                }
                catch (Exception exception)
                {
                    _connection.Dispose();
                    Debug.WriteLine("Close Error", "SqliteDataBase");
                    LogRecorder.Error(exception.ToString());
                }
                finally
                {
                    if (_default == this)
                        _default = null;
                }
            }
        }
        /// <summary>
        ///     连接对象
        /// </summary>
        public SQLiteConnection GetCurrentConnection()
        {
            return Connection;
        }

        /// <summary>
        ///     连接对象
        /// </summary>
        public void ClearCurrentConnection()
        {
            _connection = null;
        }


        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        protected virtual void DoDispose()
        {
        }


        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            DoDispose();
            Close();
            GC.ReRegisterForFinalize(this);
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
            //using (SqliteDataBaseScope.CreateScope(this))
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
        public int Execute(string sql, IEnumerable<SQLiteParameter> args)
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
        public int Execute(string sql, params SQLiteParameter[] args)
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
        public object ExecuteScalar(string sql, IEnumerable<SQLiteParameter> args)
        {
            //using (SqliteDataBaseScope.CreateScope(this))
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
        public object ExecuteScalar(string sql, params SQLiteParameter[] args)
        {
            //using (SqliteDataBaseScope.CreateScope(this))
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
            //using (SqliteDataBaseScope.CreateScope(this))
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
            //using (SqliteDataBaseScope.CreateScope(this))
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
        public T ExecuteScalar<T>(string sql, params SQLiteParameter[] args)
        {
            //using (SqliteDataBaseScope.CreateScope(this))
            {
                return ExecuteScalarInner<T>(sql, args);
            }
        }


        /// <summary>
        ///     清除所有数据
        /// </summary>
        public void Clear(string table)
        {
            Execute(string.Format(@"DELETE FROM [{0}];
UPDATE sqlite_sequence SET seq = 0 WHERE name = '{0}';", table));
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
        protected int ExecuteInner(string sql, params SQLiteParameter[] args)
        {
            lock (this)
            {
                int result;
                using (var cmd = CreateCommand(sql, args))
                {
                    result = cmd.ExecuteNonQuery();
                }
                return result;
            }
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
        protected object ExecuteScalarInner(string sql, params SQLiteParameter[] args)
        {
            lock (this)
            {
                object result;
                using (var cmd = CreateCommand(sql, args))
                {
                    result = cmd.ExecuteScalar();
                }
                return result == DBNull.Value ? null : result;
            }
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
        protected T ExecuteScalarInner<T>(string sql, params SQLiteParameter[] args)
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
        public static void TraceSql(SQLiteCommand cmd)
        {
            if (!LogRecorder.LogDataSql)
                return;
            TraceSql(cmd.CommandText, cmd.Parameters.OfType<SQLiteParameter>());
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
        public static void TraceSql(string sql, IEnumerable<SQLiteParameter> args)
        {
            if (!LogRecorder.LogDataSql)
                return;
            StringBuilder code = new StringBuilder();
            code.AppendLine("/***************************************************************/");
            var parameters = args as SQLiteParameter[] ?? args.ToArray();
            foreach (var par in parameters.Where(p => p != null))
            {
                code.AppendLine($"declare ?{par.ParameterName} {par.DbType};");
            }
            foreach (var par in parameters.Where(p => p != null))
            {
                code.AppendLine($"SET ?{par.ParameterName} = '{par.Value}';");
            }
            code.AppendLine(sql);
            LogRecorder.RecordDataLog(code.ToString());
        }


        #endregion

        #region 生成命令对象

        /// <summary>
        ///     生成命令
        /// </summary>
        public SQLiteCommand CreateCommand(params SQLiteParameter[] args)
        {
            return CreateCommand(null, args);
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        public SQLiteCommand CreateCommand(string sql, SQLiteParameter arg)
        {
            return CreateCommand(sql, new[] { arg });
        }

        /// <summary>
        ///     生成命令
        /// </summary>
        public SQLiteCommand CreateCommand(string sql, IEnumerable<SQLiteParameter> args = null)
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
                var parameters = args as SQLiteParameter[] ?? args.ToArray();
                if (parameters.Any(p => p != null))
                {
                    cmd.Parameters.AddRange(
                        parameters.Where(p => p != null)
                            .Select(
                                p =>
                                    new SQLiteParameter(p.ParameterName, p.DbType, p.Size, p.Direction, p.IsNullable, p.Precision, p.Scale,
                                        p.SourceColumn, p.SourceVersion, p.Value)).ToArray());
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
        public Dictionary<string, TableSql> TableSql => _tableSql ?? (_tableSql = new Dictionary<string, TableSql>(StringComparer.OrdinalIgnoreCase));

        #endregion

        #region 生成Sql参数

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="csharpType">C#的类型</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static SQLiteParameter CreateParameter(string csharpType, string parameterName, object value)
        {
            if (value is Enum)
            {
                return new SQLiteParameter(parameterName, DbType.Int32)
                {
                    Value = Convert.ToInt32(value)
                };
            }
            if (value is bool b)
            {
                return new SQLiteParameter(parameterName, DbType.Byte)
                {
                    Value = b ? 1 : 0
                };
            }
            return CreateParameter(parameterName, value, ToSqlDbType(csharpType));
        }


        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">类型</param>
        /// <returns>参数</returns>
        public static SQLiteParameter CreateParameter(string parameterName, object value, DbType type)
        {
            switch (value)
            {
                case null:
                    return new SQLiteParameter(parameterName, DbType.String)
                    {
                        Value = DBNull.Value
                    };
                case string s:
                    return CreateParameter(parameterName, s);
                case Enum _:
                    return new SQLiteParameter(parameterName, DbType.Int32)
                    {
                        Value = Convert.ToInt32(value)
                    };
                case bool _:
                    return new SQLiteParameter(parameterName, DbType.Byte)
                    {
                        Value = (bool)value ? 1 : 0
                    };
            }

            return new SQLiteParameter(parameterName, type)
            {
                Value = value
            };
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static SQLiteParameter CreateParameter(string parameterName, object value)
        {
            return CreateParameter(parameterName, value, ToSqlDbType(value?.GetType().Name));
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static SQLiteParameter CreateParameter(string parameterName, string value)
        {
            if (value == null)
            {
                return new SQLiteParameter(parameterName, DbType.String, 10);
            }
            return new SQLiteParameter(parameterName, DbType.String, value.Length)
            {
                Value = value
            };
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static SQLiteParameter CreateParameter(string parameterName, bool value)
        {
            return new SQLiteParameter(parameterName, DbType.Byte)
            {
                Value = value ? 1 : 0
            };
        }

        /// <summary>
        ///     生成Sql参数
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        public static SQLiteParameter CreateParameter<T>(string parameterName, T value)
            where T : struct
        {
            if (value is Enum)
            {
                return new SQLiteParameter(parameterName, DbType.Int32)
                {
                    Value = Convert.ToInt32(value)
                };
            }
            return new SQLiteParameter(parameterName, ToSqlDbType(typeof(T).Name))
            {
                Value = value
            };
        }


        /// <summary>
        ///     从C#的类型转为DBType
        /// </summary>
        /// <param name="csharpType"> </param>
        public static DbType ToSqlDbType(string csharpType)
        {
            switch (csharpType)
            {
                case "Boolean":
                case "bool":
                    return DbType.Boolean;
                case "byte":
                case "Byte":
                case "sbyte":
                case "SByte":
                case "Char":
                case "char":
                    return DbType.Byte;
                case "short":
                case "Int16":
                case "ushort":
                case "UInt16":
                    return DbType.Int16;
                case "int":
                case "Int32":
                case "IntPtr":
                case "uint":
                case "UInt32":
                case "UIntPtr":
                    return DbType.Int32;
                case "long":
                case "Int64":
                case "ulong":
                case "UInt64":
                    return DbType.Int64;
                case "float":
                case "Float":
                    return DbType.Single;
                case "double":
                case "Double":
                    return DbType.Double;
                case "decimal":
                case "Decimal":
                    return DbType.Decimal;
                case "Guid":
                    return DbType.Guid;
                case "DateTime":
                    return DbType.DateTime;
                case "String":
                case "string":
                    return DbType.String;
                case "Binary":
                case "byte[]":
                case "Byte[]":
                    return DbType.Binary;
                default:
                    return DbType.Binary;
            }
        }

        /// <summary>
        ///     从C#的类型转为DBType
        /// </summary>
        /// <param name="csharpType"> </param>
        public static DbType ToDbType(string csharpType)
        {
            switch (csharpType)
            {
                case "Boolean":
                case "bool":
                    return DbType.Boolean;
                case "byte":
                case "Byte":
                    return DbType.Byte;
                case "sbyte":
                case "SByte":
                    return DbType.SByte;
                case "short":
                case "Int16":
                    return DbType.Int16;
                case "ushort":
                case "UInt16":
                    return DbType.UInt16;
                case "int":
                case "Int32":
                case "IntPtr":
                    return DbType.Int32;
                case "uint":
                case "UInt32":
                case "UIntPtr":
                    return DbType.UInt32;
                case "long":
                case "Int64":
                    return DbType.Int64;
                case "ulong":
                case "UInt64":
                    return DbType.UInt64;
                case "float":
                case "Float":
                    return DbType.Single;
                case "double":
                case "Double":
                    return DbType.Double;
                case "decimal":
                case "Decimal":
                    return DbType.Decimal;
                case "Guid":
                    return DbType.Guid;
                case "DateTime":
                    return DbType.DateTime;
                case "Binary":
                case "byte[]":
                case "Byte[]":
                    return DbType.Binary;
                case "string":
                case "String":
                    return DbType.String;
                default:
                    return DbType.String;
            }
        }

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
            return Execute(sql, args.OfType<SQLiteParameter>());
        }

        int IDataBase.Execute(string sql, params DbParameter[] args)
        {
            return Execute(sql, args.OfType<SQLiteParameter>());
        }

        object IDataBase.ExecuteScalar(string sql, IEnumerable<DbParameter> args)
        {
            return ExecuteScalar(sql, args.OfType<SQLiteParameter>());
        }

        object IDataBase.ExecuteScalar(string sql, params DbParameter[] args)
        {
            return ExecuteScalar(sql, args.OfType<SQLiteParameter>().ToArray());
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
            return ExecuteScalar<T>(sql, args.OfType<SQLiteParameter>().ToArray());
        }

        DbCommand IDataBase.CreateCommand(params DbParameter[] args)
        {
            return CreateCommand(args.OfType<SQLiteParameter>().ToArray());
        }

        DbCommand IDataBase.CreateCommand(string sql, DbParameter arg)
        {
            return CreateCommand((SQLiteParameter)arg);
        }

        DbCommand IDataBase.CreateCommand(string sql, IEnumerable<DbParameter> args)
        {
            return CreateCommand(sql, args.OfType<SQLiteParameter>().ToArray());
        }



        #endregion
    }

}