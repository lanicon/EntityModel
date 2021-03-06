using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Agebull.Common.Ioc;
using Agebull.EntityModel.Common;

namespace Agebull.EntityModel.Redis
{
    /// <summary>
    /// REDIS代理类
    /// </summary>
    public class RedisProxyAsync : IDisposable
    {
        #region 测试支持
        /// <summary>
        /// 测试支持
        /// </summary>
        public static bool IsTest { get; set; }

        #endregion

        #region 构造


        /// <summary>
        /// 使用哪个数据库
        /// </summary>
        private int _db;

        /// <summary>
        /// 当前数据库
        /// </summary>
        public long CurrentDb => Redis.CurrentDb;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="db"></param>
        public RedisProxyAsync(int db = 0)
        {
            _db = db;
            ChangeDb();
        }

        /*// <summary>
        /// 使用中的
        /// </summary>
        private static readonly Dictionary<int, List<IRedisAsync>> Used = new Dictionary<int, List<IRedisAsync>>();
        /// <summary>
        /// 空闲的
        /// </summary>
        private static readonly Dictionary<int, List<IRedisAsync>> Idle = new Dictionary<int, List<IRedisAsync>>();*/
        /// <summary>
        /// 客户端类
        /// </summary>
        internal IRedisAsync _redis;
        /// <summary>
        /// 得到一个可用的Redis客户端
        /// </summary>
        public IRedisAsync Redis
        {
            get
            {
                if (_redis != null)
                    return _redis;
                return _redis = CreateClient();
            }
        }

        /// <summary>
        /// 更改
        /// </summary>
        internal IRedisAsync ChangeDb(int db)
        {
            _db = db;
            return ChangeDb();
        }

        /// <summary>
        /// 更改
        /// </summary>
        private IRedisAsync ChangeDb()
        {
            if (_redis == null)
            {
                var old = _redis;
                _redis = CreateClient();
                return old;
            }
            if (_db == _redis.CurrentDb)
            {
                return _redis;
            }
            _redis.ChangeDb(_db);
            return _redis;
        }

        /// <summary>
        /// 更改
        /// </summary>
        internal void ResetClient(IRedisAsync client)
        {
            _redis = client;
        }

        private IRedisAsync CreateClient()
        {
            _redis = IocHelper.Create<IRedisAsync>();
            if (_redis == null)
                throw new Exception("未正确注册Redis对象");
            ChangeDb();
            return _redis;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_redis == null || _redis.NoClose)
                return;
            _redis.Dispose();
            _redis = null;
        }


        #endregion

        #region 文本读写

        /// <summary>
        /// 删除KEY
        /// </summary>
        /// <param name="key"></param>
        public Task RemoveKey(string key)
        {
            return Redis.RemoveKey(key);
        }

        /// <summary>
        /// 查找关删除KEY
        /// </summary>
        /// <param name="pattern">条件</param>
        public Task FindAndRemoveKey(string pattern)
        {
            return Redis.FindAndRemoveKey(pattern);
        }

        /// <summary>
        /// 取文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<string> Get(string key)
        {
            return Redis.Get<string>(key);
        }


        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task Set(string key, string value)
        {
            return Redis.Set(key, value);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public Task Set(string key, string value, DateTime last)
        {
            return Redis.Set(key, value, last);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public Task Set(string key, string value, TimeSpan span)
        {
            return Redis.Set(key, value, span);
        }

        #endregion

        #region 值读写

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<T> GetValue<T>(string key)
            where T : struct
        {
            return Redis.GetValue<T>(key);
        }
        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task SetValue<T>(string key, T value)
            where T : struct
        {
            return Redis.SetValue(key, value);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public Task SetValue<T>(string key, T value, DateTime last)
            where T : struct
        {
            return Redis.SetValue(key, value, last);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public Task SetValue<T>(string key, T value, TimeSpan span)
            where T : struct
        {
            return Redis.SetValue(key, value, span);
        }

        #endregion

        #region 对象读写


        /// <summary>
        /// 查找关删除KEY
        /// </summary>
        /// <param name="pattern">条件</param>
        public Task<List<T>> GetAll<T>(string pattern)
            where T : class
        {
            return Redis.GetAll<T>(pattern);
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <remarks>
        /// 如果反序列化失败会抛出异常
        /// </remarks>
        public Task<T> Get<T>(string key)
            where T : class
        {
            return Redis.Get<T>(key);
        }
        /// <summary>
        /// 试图取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>如果反序列化失败失败会访回空</returns>
        public Task<T> TryGet<T>(string key)
            where T : class
        {
            return Redis.TryGet<T>(key);
        }
        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task Set<T>(string key, T value)
            where T : class
        {
            return Redis.Set(key, value);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public Task Set<T>(string key, T value, DateTime last)
            where T : class
        {
            return Redis.Set(key, value, last);
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="span"></param>
        /// <returns></returns>
        public Task Set<T>(string key, T value, TimeSpan span)
            where T : class
        {
            return Redis.Set(key, value, span);
        }

        #endregion

        #region 实体缓存支持

        /// <summary>
        /// 读缓存
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="id">键的组合</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public Task<TData> GetEntity<TData>(long id, TData def = null) where TData : class, new()
        {
            return Redis.GetEntity(id, def);
        }

        /// <summary>
        /// 读缓存
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="key">数据键</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        public Task<TData> GetEntity<TData>(string key, TData def = null) where TData : class, new()
        {
            return Redis.GetEntity(key, def);
        }

        /// <summary>
        /// 缓存数据
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data">键的组合</param>
        /// <returns></returns>
        public Task SetEntity<TData>(TData data) where TData : class, IIdentityData
        {
            return Redis.SetEntity(data);
        }

        /// <summary>
        /// 缓存这些对象
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="datas">数据</param>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns></returns>
        public Task CacheData<TData>(IEnumerable<TData> datas, Func<TData, string> keyFunc = null) where TData : class, IIdentityData, new()
        {
            return Redis.CacheData(datas, keyFunc);
        }

        /// <summary>
        /// 缓存这些对象
        /// </summary>
        /// <typeparam name="TDataAccess"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns></returns>
        public Task CacheData<TData, TDataAccess>(Func<TData, string> keyFunc = null)
            where TDataAccess : IDataTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var access = new TDataAccess();
            var datas = access.All();
            return CacheData(datas, keyFunc);
        }

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <param name="lambda">查询表达式</param>
        /// <param name="keyFunc">设置键的方法,可为空</param>
        /// <returns>数据</returns>
        public async Task CacheData<TData, TDataAccess>(Expression<Func<TData, bool>> lambda, Func<TData, string> keyFunc = null)
            where TDataAccess : IDataTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            var access = new TDataAccess();
            var datas = await access.AllAsync();
            await CacheData(datas, keyFunc);
        }

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <returns>数据</returns>
        public Task ClearCache<TData>()
        {
            return FindAndRemoveKey(DataKeyBuilder.DataKey<TData>("*"));
        }

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <returns>数据</returns>
        public async Task RefreshCache<TData, TDataAccess>(long id, Func<TData, bool> lambda = null)
            where TDataAccess : IDataTable<TData>, new()
            where TData : EditDataObject, IIdentityData, new()
        {
            if (id <= 0)
                return;
            var access = new TDataAccess();
            var data = await access.LoadByPrimaryKeyAsync(id);
            if (data != null && (lambda == null || lambda(data)))
                await Set(DataKeyBuilder.DataKey(data), data);
            else
                await Redis.RemoveKey(DataKeyBuilder.DataKey<TData>(id));
        }

        /// <summary>
        ///     缓存这些对象
        /// </summary>
        /// <returns>数据</returns>
        public Task RemoveCache<TData>(long id)
        {
            return Redis.RemoveCache<TData>(id);
        }

        #endregion

    }
}