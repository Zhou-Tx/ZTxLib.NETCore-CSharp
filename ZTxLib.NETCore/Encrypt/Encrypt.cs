using System;
using System.Text;

namespace ZTxLib.NETCore.Encrypt
{
    /// <summary>
    /// Base64
    /// </summary>
    public static class Encrypt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">原串</param>
        /// <returns>密串</returns>
        public static string ToBase64(this string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">密串</param>
        /// <returns>原串</returns>
        public static string FromBase64(this string text)
        {
            byte[] bytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
