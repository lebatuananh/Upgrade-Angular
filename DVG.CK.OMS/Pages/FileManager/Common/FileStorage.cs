using DVG.WIS.Utilities;
using DVG.WIS.Utilities.Logs;
using FileManager.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace FileManager.Common
{
    public class FileStorage
    {
        public static string AESKey = Config.AESKey;
        public static string AESIV = Config.AESIV;
        public static string AESEncrypt(string Input) // "{username}|2014-12-17 10:15" 
        {
            var aes = new RijndaelManaged();
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(AESKey);
            aes.IV = Convert.FromBase64String(AESIV);

            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            return Convert.ToBase64String(xBuff);
        }

        public string AESDecrypt(string Input)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(AESKey);
            aes.IV = Convert.FromBase64String(AESIV);

            var decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            return Encoding.UTF8.GetString(xBuff);
        }
        public static string EncriptUsername(string username)
        {
            var key = AESEncrypt(username);
            key = key.Replace("/", "")
                     .Replace("\\", "")
                     .Replace("?", "")
                     .Replace("+", "")
                     .Replace("=", "");
            return key.Substring(0, 8);
        }
        public static string SendRequestWithParram(string url, NameValueCollection nvc)
        {
            try
            {
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.KeepAlive = true;
                wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
                wr.Proxy = null;
                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                Stream rs = wr.GetRequestStream();

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in nvc.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                WebResponse wresp = null;
                var strResult = string.Empty;
                try
                {
                    wresp = wr.GetResponse();
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);
                    strResult = reader2.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Error, ex);
                    if (wresp != null)
                    {
                        wresp.Close();
                        wresp = null;
                    }
                }
                finally
                {
                    wr = null;
                }

                return strResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex);
                return string.Empty;
            }
        }
        public static string HttpUploadFile(string url, IFormFile file, string paramName, string contentType, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            wr.Proxy = null;
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file.FileName, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            var fileStream = file.OpenReadStream();

            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            var strResult = string.Empty;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                strResult = reader2.ReadToEnd();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }

            return strResult;
        }

        //public static string HttpUploadFile(string url, HttpPostedFile file, string paramName, string contentType, NameValueCollection nvc)
        //{
        //	string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
        //	byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

        //	HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
        //	wr.ContentType = "multipart/form-data; boundary=" + boundary;
        //	wr.Method = "POST";
        //	wr.KeepAlive = true;
        //	wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
        //	wr.Proxy = null;
        //	ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

        //	Stream rs = wr.GetRequestStream();

        //	string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
        //	foreach (string key in nvc.Keys)
        //	{
        //		rs.Write(boundarybytes, 0, boundarybytes.Length);
        //		string formitem = string.Format(formdataTemplate, key, nvc[key]);
        //		byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
        //		rs.Write(formitembytes, 0, formitembytes.Length);
        //	}
        //	rs.Write(boundarybytes, 0, boundarybytes.Length);

        //	string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
        //	string header = string.Format(headerTemplate, paramName, file.FileName, contentType);
        //	byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
        //	rs.Write(headerbytes, 0, headerbytes.Length);

        //	var fileStream = file.InputStream;
        //	byte[] buffer = new byte[4096];
        //	int bytesRead = 0;
        //	while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
        //	{
        //		rs.Write(buffer, 0, bytesRead);
        //	}
        //	fileStream.Close();

        //	byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
        //	rs.Write(trailer, 0, trailer.Length);
        //	rs.Close();

        //	WebResponse wresp = null;
        //	var strResult = string.Empty;
        //	try
        //	{
        //		wresp = wr.GetResponse();
        //		Stream stream2 = wresp.GetResponseStream();
        //		StreamReader reader2 = new StreamReader(stream2);
        //		strResult = reader2.ReadToEnd();
        //	}
        //	catch (Exception ex)
        //	{
        //		Logger.WriteLog(Logger.LogType.Error, ex);
        //		if (wresp != null)
        //		{
        //			wresp.Close();
        //			wresp = null;
        //		}
        //	}
        //	finally
        //	{
        //		wr = null;
        //	}

        //	return strResult;
        //}
        //public static string HttpUploadFile(string url, HttpPostedFileBase file, string paramName, string contentType, NameValueCollection nvc)
        //{
        //	string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
        //	byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

        //	HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
        //	wr.ContentType = "multipart/form-data; boundary=" + boundary;
        //	wr.Method = "POST";
        //	wr.KeepAlive = true;
        //	wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
        //	wr.Proxy = null;
        //	ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

        //	Stream rs = wr.GetRequestStream();

        //	string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
        //	foreach (string key in nvc.Keys)
        //	{
        //		rs.Write(boundarybytes, 0, boundarybytes.Length);
        //		string formitem = string.Format(formdataTemplate, key, nvc[key]);
        //		byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
        //		rs.Write(formitembytes, 0, formitembytes.Length);
        //	}
        //	rs.Write(boundarybytes, 0, boundarybytes.Length);

        //	string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
        //	string header = string.Format(headerTemplate, paramName, file.FileName, contentType);
        //	byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
        //	rs.Write(headerbytes, 0, headerbytes.Length);

        //	var fileStream = file.InputStream;
        //	byte[] buffer = new byte[4096];
        //	int bytesRead = 0;
        //	while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
        //	{
        //		rs.Write(buffer, 0, bytesRead);
        //	}
        //	fileStream.Close();

        //	byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
        //	rs.Write(trailer, 0, trailer.Length);
        //	rs.Close();

        //	WebResponse wresp = null;
        //	var strResult = string.Empty;
        //	try
        //	{
        //		wresp = wr.GetResponse();
        //		Stream stream2 = wresp.GetResponseStream();
        //		StreamReader reader2 = new StreamReader(stream2);
        //		strResult = reader2.ReadToEnd();
        //	}
        //	catch (Exception ex)
        //	{
        //		Logger.WriteLog(Logger.LogType.Error, ex);
        //		if (wresp != null)
        //		{
        //			wresp.Close();
        //			wresp = null;
        //		}
        //	}
        //	finally
        //	{
        //		wr = null;
        //	}

        //	return strResult;
        //}
        public static bool CopyOutFromTempFolder(string fileName, out string newImages)
        {
            newImages = string.Empty;
            try
            {
                // Generate post objects
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("FileTemp", fileName);
                postParameters.Add("project", Config.UploadProject);
                postParameters.Add("UploadType", "copy");
                var eKey = AESEncrypt(string.Concat("temp|", DateTime.Now.ToString("yyyy-MM-dd HH:mm")));
                postParameters.Add("StringDecypt", eKey);
                postParameters.Add("param", "end");

                string postURL = string.Concat(Config.UploadDomain, Config.UploadHandler);
                string userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.1) Gecko/20060111 Firefox/1.5.0.1";
                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters, string.Empty);

                // Process response
                if (webResponse != null)
                {
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    string fullResponse = responseReader.ReadToEnd();
                    webResponse.Close();
                    //return readImageHost + baseFolder + "/" + fu.FileName;                    
                    return GetImageURLFromResult(fullResponse, out newImages);
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex);
                return false;
            }
        }
        public static bool DownloadFromRotateFolder(string fileName, out string newImages)
        {
            newImages = string.Empty;
            try
            {
                // Generate post objects
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("downloadUrl", string.Concat(Config.ViewDomain, "/", fileName));
                postParameters.Add("project", Config.UploadProject);
                postParameters.Add("UploadType", "downloadFileFromUrl");
                var eKey = AESEncrypt(string.Concat("|", DateTime.Now.ToString("yyyy-MM-dd HH:mm")));
                postParameters.Add("StringDecypt", eKey);
                postParameters.Add("submit", "Upload Image");
                postParameters.Add("param", "end");

                string postURL = string.Concat(Config.UploadDomain, Config.UploadHandler);
                string userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.1) Gecko/20060111 Firefox/1.5.0.1";
                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters, string.Empty);

                // Process response
                if (webResponse != null)
                {
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    string fullResponse = responseReader.ReadToEnd();
                    webResponse.Close();
                    //return readImageHost + baseFolder + "/" + fu.FileName;                    
                    return GetImageURLFromResult(fullResponse, out newImages);
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex);
                return false;
            }
        }
        private static bool GetImageURLFromResult(string fullResponse, out string newImages)
        {
            if (fullResponse.IndexOf("OK") != -1)
            {
                newImages = fullResponse.Replace("{\"OK\": \"", string.Empty).Replace("\"}", string.Empty);
                return true;
            }
            else
            {
                newImages = string.Empty;
                return false;
            }
        }
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}