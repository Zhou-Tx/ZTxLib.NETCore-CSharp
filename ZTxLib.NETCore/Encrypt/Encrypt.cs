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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UTF8Encode(this string text)
        {
            string result = string.Empty;
            foreach (byte v in Encoding.UTF8.GetBytes(text))
                result += v.ToString("X2");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UTF8Decode(this string text)
        {
            int len = text.Length / 2;
            byte[] bytes = new byte[len];
            for (int i = 0; i < len; i++)
            {
                string tmp = text.Substring(i * 2, 2);
                bytes[i] = (byte)Convert.ToInt32(tmp, 16);
            }
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
