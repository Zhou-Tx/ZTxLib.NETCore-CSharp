using System.IO;
using System.Net;
using System.Text;

namespace ZTxLib.NETCore
{
    /// <summary>
    /// 基于超文本传输协议的相关操作
    /// </summary>
    public static class HTTP
    {
        /// <summary>
        /// 通过GET方式向URL发送请求（Request），返回响应结果（Response）
        /// </summary>
        /// <param name="url">统一资源定位符</param>
        /// <param name="data">通过GET方式传送的参数</param>
        /// <returns>响应结果</returns>
        public static string Get(string url, string data = "")
        {
            WebRequest request = WebRequest.Create(url + "?" + data);
            request.Method = "GET";
            StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream());
            return sr.ReadToEnd();
        }

        /// <summary>
        /// 通过POST方式向URL发送请求（Request），返回响应结果（Response）
        /// </summary>
        /// <param name="url">统一资源定位符</param>
        /// <param name="data">通过POST方式传送的参数</param>
        /// <returns>响应结果</returns>
        public static string Post(string url, string data = "")
        {
            ServicePointManager.Expect100Continue = false;
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            request.ContentLength = bytes.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream());
            return sr.ReadToEnd();
        }

        /// <summary>
        /// 通过HTTP协议下载文件
        /// </summary>
        /// <param name="url">网络上的文件位置</param>
        /// <param name="path">保存到本地的地址</param>
        public static void Download(string url, string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //创建本地文件写入流
            Stream stream = new FileStream(path, FileMode.Create);
            byte[] bArr = new byte[1024 * 1024];
            int size = responseStream.Read(bArr, 0, bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, bArr.Length);
            }
            stream.Close();
            responseStream.Close();
        }
    }
}
