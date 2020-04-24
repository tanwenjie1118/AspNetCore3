using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Infrastructure.Helpers
{
    public class ObjectHelper
    {
        /// <summary>
        /// TEntity转换为STRING对象 by UTF-8
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ToString(object obj)
        {
            using var ms = new MemoryStream();
            IFormatter iFormatter = new BinaryFormatter();
            iFormatter.Serialize(ms, obj);
            var buffer = ms.GetBuffer();
            var str = Encoding.UTF8.GetString(buffer);
            return str;
        }

        /// <summary>
        /// Byte[]转换为STRING对象 by UTF-8
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ToString(byte[] buffer)
        {
            var str = Encoding.UTF8.GetString(buffer);
            return str;
        }

        /// <summary>
        /// STRING转换为TEntity对象 by UTF-8
        /// </summary>
        /// <typeparam name="str"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEntity ToEntity<TEntity>(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            using var ms = new MemoryStream(buffer);
            IFormatter iFormatter = new BinaryFormatter();
            return (TEntity)iFormatter.Deserialize(ms);
        }

        /// <summary>
        /// STRING 转换为Byte[] by UTF-8
        /// </summary>
        /// <typeparam name="str"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            return buffer;
        }
    }
}
