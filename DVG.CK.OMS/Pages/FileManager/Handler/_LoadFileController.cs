using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using DVG.WIS.Business.Authenticator;
using DVG.CK.OMS.Pages.FileManager.Entity;
using DVG.WIS.Utilities.Serialization;
using FileManager.Common;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DVG.CK.OMS.Pages.FileManager.Handler
{

    public class _LoadFileController : Controller
    {
        private IUserService _userService;

        public _LoadFileController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost]
        public JsonResult Index(string path)
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
                string userName = _userService.GetUserLogin().UserName;
                string key = FileStorage.AESEncrypt(userName + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                string folder = string.Concat(path, "/", FileStorage.EncriptUsername(userName));
                NameValueCollection nvc = new NameValueCollection()
                {
                    { "project", Config.UploadProject },
                    { "folder", folder },
                    { "StringDecypt", key },
                    { "submit", "Check" }
                };
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
    }
}
