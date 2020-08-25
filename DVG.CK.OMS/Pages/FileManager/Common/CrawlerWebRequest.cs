using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace FileManager.Common
{
    public class CrawlerWebRequest
    {
        public string Method { get; set; }
        public bool KeepAlive { get; set; }
        public int Timeout { get; set; }
        public string UserAgent { get; set; }
        public string Referer { get; set; }
        public string Accept { get; set; }
        public string ContentType { get; set; }
        public string Domain { get; set; }
        public DecompressionMethods AutomaticDecompression { get; set; }

        public CrawlerWebRequest()
        {
            this.Method = "GET";
            this.KeepAlive = true;
            this.Timeout = 60000;
            this.Accept = "application/json, text/javascript, */*; q=0.01";
            this.ContentType = "application/x-www-form-urlencoded";
            this.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
        }

        public bool GetHtml(out string html, out HttpStatusCode status)
        {
            html = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(this.Domain))
                {
                    status = HttpStatusCode.NotFound;
                }

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(this.Domain);

                request.Method = this.Method;
                request.UserAgent = this.UserAgent;
                request.ContentType = this.ContentType;
                request.KeepAlive = this.KeepAlive;
                request.Timeout = this.Timeout;
                request.Referer = this.Referer;
                request.Accept = this.Accept;
                request.AutomaticDecompression = this.AutomaticDecompression;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());

                status = response.StatusCode;
                html = sr.ReadToEnd();

                return true;
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;

                status = response.StatusCode;
            }
            catch
            {
                status = HttpStatusCode.Forbidden;
            }

            return false;
        }

        public bool GetHtml(string data, out string html, out HttpStatusCode status)
        {
            html = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(this.Domain))
                {
                    status = HttpStatusCode.NotFound;
                }

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(this.Domain);
                byte[] dataSend = Encoding.UTF8.GetBytes(data);

                request.Method = this.Method;
                request.UserAgent = this.UserAgent;
                request.ContentType = this.ContentType;
                request.KeepAlive = this.KeepAlive;
                request.Timeout = this.Timeout;
                request.Referer = this.Referer;
                request.Accept = this.Accept;
                request.ContentLength = dataSend.Length;
                request.AutomaticDecompression = this.AutomaticDecompression;

                Stream stream = request.GetRequestStream();

                stream.Write(dataSend, 0, dataSend.Length);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());

                status = response.StatusCode;
                html = sr.ReadToEnd();

                return true;
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;

                status = response.StatusCode;
            }
            catch
            {
                status = HttpStatusCode.Forbidden;
            }

            return false;
        }
    }
}