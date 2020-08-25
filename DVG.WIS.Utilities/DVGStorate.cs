using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace DVG.WIS.Utilities
{
    public class DVGStorate
    {

        public static string _UploadDomain = AppSettings.Instance.GetString("UploadDomain");
        public static string _UploadDomainSvr = AppSettings.Instance.GetString("UploadDomainSvr");
        public static string _UploadProject = AppSettings.Instance.GetString("UploadProject");
        public static string _UploadHandler = AppSettings.Instance.GetString("UploadHandler");
        public static string _ViewDomain = AppSettings.Instance.GetString("DomainAvatar");
        public static string _NoImage = AppSettings.Instance.GetString("NoImage");
        public static string AES_Key = AppSettings.Instance.GetString("AES-Key");
        public static string AES_IV = AppSettings.Instance.GetString("AES-IV");
        public static string VideoAES_Key = AppSettings.Instance.GetString("Video-AES-Key");
        public static string VideoAES_IV = AppSettings.Instance.GetString("Video-AES-IV");
        public static string _VideoUploadDomain = AppSettings.Instance.GetString("Video-Upload-Domain");
        public static string _VideoUploadHandler = AppSettings.Instance.GetString("Video-Upload-Handler");

        public static class UploadType
        {
            public static string Upload = "upload";
            public static string Copy = "copy";
            public static string Download = "downloadFileFromUrl";
        }

        public static string AES_encrypt(string Input) // "{username}|2014-12-17 10:15" 
        {
            var aes = new RijndaelManaged();
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = AES_Key.ToByteArray();
            aes.IV = AES_IV.ToByteArray();

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
        public string AES_decrypt(string Input)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(AES_Key);
            aes.IV = Convert.FromBase64String(AES_IV);

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
            var key = AES_encrypt(username);
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
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        public static bool CopyOutFromTempFolder(string fileName, out string newImages)
        {
            newImages = string.Empty;
            try
            {
                // Generate post objects
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("FileTemp", fileName);
                postParameters.Add("project", _UploadProject);
                postParameters.Add("UploadType", UploadType.Copy);
                var eKey = DVGStorate.AES_encrypt("" + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                postParameters.Add("StringDecypt", eKey);
                //postParameters.Add("WaterMark", "true");
                postParameters.Add("param", "end");

                string postURL = _UploadDomain + _UploadHandler;
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
                postParameters.Add("downloadUrl", AppSettings.Instance.GetString("DomainImage").TrimEnd('/') + "/" + fileName);
                postParameters.Add("project", _UploadProject);
                postParameters.Add("UploadType", UploadType.Download);
                var eKey = DVGStorate.AES_encrypt("" + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                postParameters.Add("StringDecypt", eKey);
                //postParameters.Add("WaterMark", "true");
                postParameters.Add("param", "end");

                string postURL = _UploadDomain + _UploadHandler;
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
            catch
            {
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

        //trả về list các ảnh đã được copy khỏi temp hoặc rotate
        public static List<string> GetListImageFromArray(string arrStringImage)
        {
            List<string> returnListImage = new List<string>();
            if (arrStringImage != null && arrStringImage.Length > 0)
            {
                string[] lstImg = arrStringImage.Trim().Split(',');
                if (lstImg.Length > 0)
                {
                    foreach (var img in lstImg)
                    {
                        string newImage = string.Empty;
                        //nếu ảnh bị quay thì phải dùng lệnh download
                        if (img.IndexOf("rotate/") != -1)
                        {
                            if (DownloadFromRotateFolder(img, out newImage))
                            {
                                returnListImage.Add(newImage);
                            }
                        }
                        else if (img.IndexOf("temp/") != -1)
                        {
                            if (CopyOutFromTempFolder(img, out newImage))
                            {
                                returnListImage.Add(newImage);
                            }
                        }
                        else
                        {
                            returnListImage.Add(img);
                        }
                    }
                }
            }
            return returnListImage;
        }
        public static string HttpUploadFileFromDisc(string url, string fileName, Stream inputStream, string paramName, string contentType, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

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
            //NhatHD: 26-12-2014
            //string header = string.Format(headerTemplate, paramName, file.FileName, contentType);
            string header = string.Format(headerTemplate, paramName, fileName, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            //NhatHD 26-12-2014
            //var fileStream = file.InputStream;
            var fileStream = inputStream;
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
    }
}
