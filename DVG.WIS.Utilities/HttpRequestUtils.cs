using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using DVG.WIS.Utilities.Logs;
using Newtonsoft.Json;

namespace DVG.WIS.Utilities
{
    public class HttpRequestUtils
    {
        static HttpRequestUtils()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public static T GetRequest<T>(string urlRequest) where T : class
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlRequest);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36";
            //webRequest.Timeout = 30000;
            webRequest.Method = "GET";

            try
            {
                WebResponse webResponse = webRequest.GetResponse();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                var content = sr.ReadToEnd().Trim();
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (WebException ex)
            {
                Logger.ErrorLog(ex);
                return default(T);
            }
        }

        public static string GetRequest(string urlRequest, out HttpStatusCode statusCode, out string errorMessage)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlRequest);
            // webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36";
            //webRequest.Timeout = 30000;
            webRequest.Method = "GET";

            try
            {
                //if (isNewTls)
                //{
                //    ServicePointManager.Expect100Continue = true;
                //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //}
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                var content = sr.ReadToEnd().Trim();
                errorMessage = string.Empty;
                statusCode = webResponse.StatusCode;
                return content;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var webResponse = (HttpWebResponse)ex.Response;
                    statusCode = webResponse.StatusCode;
                    errorMessage = webResponse.StatusDescription;
                }
                else
                {
                    Logger.ErrorLog(ex);
                    statusCode = HttpStatusCode.BadRequest;
                    errorMessage = ex.Message;
                }

                return string.Empty;
            }
        }

        public static T PostRequest<T>(string uri, string parameters, out string content) where T : class, new()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36";
            byte[] bytes = Encoding.UTF8.GetBytes(parameters);
            webRequest.ContentLength = bytes.Length;
            using (Stream os = webRequest.GetRequestStream())
            {
                os.Write(bytes, 0, bytes.Length);
            }

            try
            {
                WebResponse webResponse = webRequest.GetResponse();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                content = sr.ReadToEnd().Trim();
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
            }
            content = string.Empty;
            return default(T);
        }

        public static string PostRequest(string url, string parameters, out HttpStatusCode statusCode, out string errorMessage)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
            //webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            webRequest.Headers.Add("Accept-Language", "vi,en;q=0.9,hi;q=0.8");
            webRequest.AllowAutoRedirect = false;

            //webRequest.Referer = ICSDomain + "/";
            webRequest.KeepAlive = true;
            byte[] bytes = Encoding.UTF8.GetBytes(parameters);

            //string content;
            try
            {
                webRequest.ContentLength = bytes.Length;
                using (Stream os = webRequest.GetRequestStream())
                {
                    os.Write(bytes, 0, bytes.Length);
                }

                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                //webResponse.GetResponseStream();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                statusCode = webResponse.StatusCode;
                errorMessage = string.Empty;
                return sr.ReadToEnd().Trim();
                //return webResponse;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var webResponse = (HttpWebResponse)ex.Response;
                    statusCode = webResponse.StatusCode;
                    errorMessage = webResponse.StatusDescription;
                }
                else
                {
                    Logger.ErrorLog(ex);
                    statusCode = HttpStatusCode.BadRequest;
                    errorMessage = ex.Message;
                }
            }

            return null;
        }
    }
}
