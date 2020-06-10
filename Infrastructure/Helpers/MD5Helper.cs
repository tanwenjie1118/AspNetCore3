using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Hal.Infrastructure.Helpers
{
    /// <summary>
    /// MD5Helper
    /// </summary>
    public class MD5Helper
    {
        /// <summary>
        /// MD5 Encrypt
        /// </summary>
        /// <param name="content">STRING</param>
        /// <returns></returns>
        public static string MD5Encrypt(string content)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(Encoding.Default.GetBytes(content));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            return tmp.ToString();
        }
    }
}
