using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using DVG.CK.OMS.Pages.FileManager.Entity;
using DVG.WIS.Utilities.Serialization;
using FileManager.Common;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DVG.CK.OMS.Pages.FileManager.Handler
{
    [Route("api/[controller]")]
    public class LoadFileController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public JsonResult Post(string path)
        {
            ResultReturn rr = new ResultReturn()
            {
                Result = true
            };

            try
            {
                //if (!HttpContext.Current.User.Identity.IsAuthenticated)
                //{
                //    throw new Exception("Bạn không có quyền truy cập.");
                //}

                DateTime dtNow = DateTime.Now;
                if (string.IsNullOrEmpty(path))
                {
                    path = dtNow.ToString("yyyy/MM/dd");
                }
                string userName = "test"; //HttpContext.Current.User.Identity.Name;
                string key = FileStorage.AESEncrypt(userName + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                string folder = string.Concat(path, "/", FileStorage.EncriptUsername(userName));
                NameValueCollection nvc = new NameValueCollection()
                {
                    { "project", Config.UploadProject },
                    { "folder", folder },
                    { "StringDecypt", key },
                    { "submit", "Check" },
                    { "EncryptType", "128" }
                };

                ////fake
                //string userName = "test"; //HttpContext.Current.User.Identity.Name;
                //string key = "QkYt1BGbZAKfBR7vh7najhRzU0Kk6Nwhr5b/qqDm2IQ=";
                //string folder = string.Concat(path, "/", FileStorage.EncriptUsername(userName));
                //NameValueCollection nvc = new NameValueCollection()
                //{
                //    { "project", Config.UploadProject },
                //    { "folder", folder },
                //    { "StringDecypt", key },
                //    { "submit", "Check" },
                //    { "EncryptType", "128" }
                //};
                //fake


                string result = FileStorage.SendRequestWithParram(Config.FullLoadFileApi, nvc);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    IList<string> images = NewtonJson.Deserialize<IList<string>>(result);
                    rr.FileInfos = new List<FileInfo>();

                    foreach (string image in images)
                    {
                        string virtualPath = string.Concat(folder, "/", image);
                        string fullPath = string.Concat(Config.ViewDomain, virtualPath);

                        rr.FileInfos.Add(new FileInfo()
                        {
                            Result = true,
                            Name = image,
                            Path = virtualPath,
                            FullPath = fullPath,
                            FullOriginalPath = fullPath
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                rr.Result = false;
                rr.Message = ex.Message;
            }
            return Json(rr);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
