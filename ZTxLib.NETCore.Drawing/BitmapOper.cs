using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ZTxLib.NETCore.Drawing
{
    /// <summary>
    /// Bitmap各表达形式转换
    /// </summary>
    public static class BitmapOper
    {
        /// <summary>
        /// 将位图转为字节数组
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="format">ImageFormat</param>
        /// <returns></returns>
        public static byte[] ToBytes(this Bitmap bmp, ImageFormat format)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms, format);
                return ms.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                ms.Close();
            }
        }

        /// <summary>
        /// 将字节数组转为Base64编码字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="format">ImageFormat</param>
        /// <returns></returns>
        public static string ToString(this byte[] bytes, ImageFormat format)
        {
            return $"data:image/{ format.ToString().ToLower()};base64,{Convert.ToBase64String(bytes)}";
        }

        /// <summary>
        /// 将位图转为Base64编码字符串(ImageUrl)
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="format">ImageFormat</param>
        /// <returns></returns>
        public static string ToString(this Bitmap bmp, ImageFormat format)
        {
            byte[] bytes = bmp.ToBytes(format);
            return bytes.ToString(format);
        }

        /// <summary>
        /// 将字节数组转为位图
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Bitmap ToBmp(this byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            try
            {
                Bitmap bmp = new Bitmap(ms);
                return bmp;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                ms.Close();
            }
        }

        /// <summary>
        /// 将Base64编码字符串(非ImageUrl)转为字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str) => Convert.FromBase64String(str);

        /// <summary>
        /// 将Base64编码字符串(非ImageUrl)转为位图
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Bitmap ToBmp(this string str) => ToBmp(Convert.FromBase64String(str));
    }
}