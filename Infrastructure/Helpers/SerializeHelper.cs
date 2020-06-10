using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Hal.Infrastructure.Helpers
{
    public class SerializeHelper
    {
        /// <summary>
        /// 序列化 byte
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static byte[] ToBytes(object obj)
        {
            using var ms = new MemoryStream();
            IFormatter iFormatter = new BinaryFormatter();
            iFormatter.Serialize(ms, obj);
            return ms.GetBuffer();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEntity ToEntity<TEntity>(byte[] buffer)
        {
            using var ms = new MemoryStream(buffer);
            IFormatter iFormatter = new BinaryFormatter();
            return (TEntity)iFormatter.Deserialize(ms);
        }
    }
}
