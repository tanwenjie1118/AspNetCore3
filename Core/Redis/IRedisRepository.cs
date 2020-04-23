

namespace Core.Redis
{
    public interface IRedisRepository
    {
        /// <summary>
        /// SetKeyValue.
        /// </summary>
        /// <param name="key">key.</param>
        /// <param name="value">value.</param>
        /// <returns></returns>
        bool SetKeyValue(string key, string value, int? keyExpire);

        /// <summary>
        /// GET key value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetKeyValue(string key);

        /// <summary>
        /// Flush.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns></returns>
        bool Flush(string key);

        /// <summary>
        /// Exists.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// SetKeyExpire.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns></returns>
        bool SetKeyExpire(string key, int? keyExpire);
    }
}