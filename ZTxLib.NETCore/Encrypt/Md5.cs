using System;
using System.Security.Cryptography;
using System.Text;

namespace ZTxLib.NETCore.Encrypt
{
    /// <summary>
    /// MD5消息摘要算法
    /// </summary>
    public static class Md5
    {
        private static readonly MD5 md5 = MD5.Create();

        /// <summary>
        /// Md5哈希摘要（16位字符串）
        /// </summary>
        /// <param name="text">原串</param>
        /// <returns>由16个字符组成的十六进制的哈希散列字符串</returns>
        public static string Md5_16(this string text)
        {
            byte[] vs = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(vs, 4, 8).Replace("-", "");
        }

        /// <summary>
        /// Md5哈希摘要（32位字符串）
        /// </summary>
        /// <param name="text">原串</param>
        /// <returns>由32个字符组成的十六进制的哈希散列字符串</returns>
        public static string Md5_32(this string text)
        {
            byte[] vs = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(vs).Replace("-", "");
        }

        /// <summary>
        /// Md5哈希摘要（64位字符串）
        /// </summary>
        /// <param name="text">原串</param>
        /// <returns>由64个字符组成的十六进制的哈希散列字符串</returns>
        public static string Md5_64(this string text)
        {
            byte[] vs = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            return Convert.ToBase64String(vs);
        }
    }
}
