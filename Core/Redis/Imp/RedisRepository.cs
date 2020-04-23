using StackExchange.Redis;
using System;

namespace Core.Redis
{
    /// <summary>
    /// Redis cache 统一仓储接口.
    /// </summary>
    /// <typeparam name="TEntity">entity class.</typeparam>
    public class RedisRepository : IRedisRepository
    {
        private IDatabase Redis { get; set; }

        /// <summary>
        /// 过期时间 单位：million seconds
        /// </summary>
        private readonly int keyExpire = 60000;

        public RedisRepository(RedisContext context)
        {
            this.Redis = context?.Redis;
        }

        /// <summary>
        /// Set key value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="keyExpire">过期时间，默认一分钟</param>
        /// <returns></returns>
        public virtual bool SetKeyValue(string key, string value, int? keyExpire)
        {
            try
            {
                TimeSpan expire;
                if (!keyExpire.HasValue)
                {
                    expire = TimeSpan.FromSeconds(60);
                }
                else
                {
                    expire = TimeSpan.FromMilliseconds(keyExpire.Value);
                }
                var res = Redis.StringSet(key, value, expire);
                return res;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// GET key value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string GetKeyValue(string key)
        {
            try
            {
                return Redis.StringGet(key);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Flush.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns></returns>
        public virtual bool Flush(string key)
        {
            try
            {
                return Redis.KeyDelete(key);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Exists.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns></returns>
        public virtual bool Exists(string key)
        {
            try
            {
                return Redis.KeyExists(key);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// SetKeyExpire.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns></returns>
        public virtual bool SetKeyExpire(string key, int? keyExpire)
        {
            try
            {
                TimeSpan expire;
                if (!keyExpire.HasValue)
                {
                    expire = TimeSpan.FromSeconds(60);
                }
                else
                {
                    expire = TimeSpan.FromMilliseconds(keyExpire.Value);
                }
                return Redis.KeyExpire(key, expire);
            }
            catch
            {
                return false;
            }
        }
    }
}
