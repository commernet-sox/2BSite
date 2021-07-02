using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WX_Site.Common
{
    public enum HttpMethod { Get, Post, Delete };
    public class HttpRequest
    {
        /// <summary>
        /// get/delete请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="requestMethod"></param>
        /// <param name="header"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string SendRequest(string url, Dictionary<string, string> data, HttpMethod requestMethod,
            Dictionary<string, string> header, int timeOut)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                if (requestMethod == HttpMethod.Get || requestMethod == HttpMethod.Delete)
                {
                    var paramStr = "";
                    foreach (var key in data.Keys)
                    {
                        paramStr += string.Format("{0}={1}&", key, HttpUtility.UrlEncode(data[key].ToString()));
                    }
                    paramStr = paramStr.TrimEnd('&');
                    if (!string.IsNullOrEmpty(paramStr))
                        url += (url.EndsWith("?") ? "&" : "?") + paramStr;
                }
                else
                {
                    throw new NotSupportedException("post方法请使用另外一个SendRequest");
                }

                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.KeepAlive = true;
                request.Timeout = timeOut;
                request.Method = requestMethod.ToString().ToUpper();
                request.Accept = "*/*";
                request.KeepAlive = true;
                request.UserAgent = "FRS-sdknet";
                request.Timeout = timeOut;
                foreach (var key in header.Keys)
                {
                    if (key == "Content-Type")
                    {
                        request.ContentType = header[key];
                    }
                    else
                    {
                        request.Headers.Add(key, header[key]);
                    }
                }
                var response = request.GetResponse();


                using (var s = response.GetResponseStream())
                {
                    var reader = new StreamReader(s, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException we)
            {
                if (we.Status == WebExceptionStatus.ProtocolError)
                {
                    using (var s = we.Response.GetResponseStream())
                    {
                        var reader = new StreamReader(s, Encoding.UTF8);
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    throw we;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="requestMethod"></param>
        /// <param name="header"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string SendRequest(string url, string postData, HttpMethod requestMethod,Dictionary<string, string> header, int timeOut = 100)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.KeepAlive = true;
                request.Timeout = timeOut;
                request.Method = requestMethod.ToString().ToUpper();
                request.Accept = "*/*";
                request.KeepAlive = true;
                request.Timeout = timeOut;
                request.ContentType = "application/json";
                foreach (var key in header.Keys)
                {
                    if (key == "Content-Type")
                    {
                        request.ContentType = header[key];
                    }
                    else
                    {
                        request.Headers.Add(key, header[key]);
                    }
                }

                var bytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = bytes.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

                var response = request.GetResponse();


                using (var s = response.GetResponseStream())
                {
                    var reader = new StreamReader(s, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException we)
            {
                if (we.Status == WebExceptionStatus.ProtocolError)
                {
                    using (var s = we.Response.GetResponseStream())
                    {
                        var reader = new StreamReader(s, Encoding.UTF8);
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    throw we;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求Url地址</param>
        /// <param name="postParameters">post提交参数</param>
        /// <returns></returns>
        public static T HttpPostDic<T>(string url, Dictionary<string, object> postParameters)
        {
            try
            {
                string retString = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/form-data";
                request.Host = "www.yanzhiceshi.com";
                request.Headers.Add("Origin", "http://www.yanzhiceshi.com");
                request.Headers.Add("Referer", "http://www.yanzhiceshi.com/");
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.101 Safari/537.36 Edg/91.0.864.48";
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                //POST参数

                var sp = Encoding.UTF8.GetBytes("-----------------------------7e33352f1074\r\n");
                var end = Encoding.UTF8.GetBytes("\r\n-----------------------------7e33352f1074--");

                request.ContentType = "multipart/form-data; boundary=---------------------------7e33352f1074";

                var rq = request.GetRequestStream();

                rq.Write(sp, 0, sp.Length);

                var dataHeader = GetKeyValueHeader("id", "");
                rq.Write(dataHeader, 0, dataHeader.Length);

                rq.Write(sp, 0, sp.Length);
                var filePath = @"C:/Users/FS/source/2BSite/2BSite/WX_Site/wwwroot/upload/images/WXSite/2021061713261896893.jpg";

                dataHeader = GetFileHeader("img", filePath);
                rq.Write(dataHeader, 0, dataHeader.Length);
                var fileData = File.ReadAllBytes(filePath);
                rq.Write(fileData, 0, fileData.Length);

                rq.Write(end, 0, end.Length);
                rq.Close();


                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream stream = response.GetResponseStream();
                    using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                    {
                        retString = streamReader.ReadToEnd().ToString();
                    }
                }
                //反序列化
                return JsonConvert.DeserializeObject<T>(retString);
            }
            catch
            {
                return default(T);
            }
        }

        private static byte[] GetKeyValueHeader(string name, string value)
        {
            string str = $"Content-Disposition: form-data; name=\"{name}\"\r\n\r\n{value}\r\n";
            return Encoding.UTF8.GetBytes(str);
        }

        private static byte[] GetFileHeader(string name, string fileName)
        {
            string str = $"Content-Disposition: form-data; name=\"{name}\"; filename=\"{fileName}\"\r\n" +
              "Content-Type: application/octet-stream\r\n\r\n";
            return Encoding.UTF8.GetBytes(str);
        }
    }
}
