using System;
using System.Security.Cryptography;
using System.Text;

namespace ZTxLib.NETCore.Encrypt
{
    /// <summary>
    /// 安全哈希算法（SHA1、SHA256、SHA384、SHA512）
    /// </summary>
    public static class Sha
    {
        /// <summary>
        /// 各哈希算法下的消息摘要算法
        /// </summary>
        /// <param name="text">原串</param>
        /// <param name="hash">哈希算法</param>
        /// <returns></returns>
        private static string Hash(string text, HashAlgorithm hash)
        {
            byte[] vs = hash.ComputeHash(Encoding.UTF8.GetBytes(text));
            return BitConverter.ToString(vs).Replace("-", "");
        }

        /// <summary>
        /// SHA1
        /// </summary>
        /// <param name="text"></param>
        /// <returns>由40个字符组成的十六进制的哈希散列字符串</returns>
        public static string Sha1(this string text) => Hash(text, SHA1.Create());

        /// <summary>
        /// SHA256
        /// </summary>
        /// <param name="text"></param>
        /// <returns>由44个字符组成的十六进制的哈希散列字符串</returns>
        public static string Sha256(this string text) => Hash(text, SHA256.Create());

        /// <summary>
        /// SHA384
        /// </summary>
        /// <param name="text"></param>
        /// <returns>由96个字符组成的十六进制的哈希散列字符串</returns>
        public static string Sha384(this string text) => Hash(text, SHA384.Create());

        /// <summary>
        /// SHA512
        /// </summary>
        /// <param name="text"></param>
        /// <returns>由128个字符组成的十六进制的哈希散列字符串</returns>
        public static string Sha512(this string text) => Hash(text, SHA512.Create());
    }
}
