using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using DVG.WIS.Utilities.Logs;
using FileManager.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DVG.CK.OMS.Controllers
{
    [Authorize]
    public class CommonController : Controller
    {
        private Message objMsg;
        public Message Msg
        {
            get
            {
                return objMsg;
            }
            set
            {
                objMsg = value;
            }
        }

        public CommonController()
        {
            //_HttpContextAccessor = new HttpContextAccessor();
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult FileManagerUpload(IFormCollection f)
        {
            Msg = new Message();
            Msg.Error = false;
            try
            {
                var uploadUrl = f["UploadUrl"] + "";
                var uploadProject = f["UploadProject"] + "";
                var uploadType = f["UploadType"] + "";
                var isWaterMark = bool.Parse(f["WaterMark"]);
                var stringDecypt = f["StringDecypt"] + "";
                var submit = f["Submit"] + "";
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files[0];
                    if (!file.ContentType.Contains("image/"))
                    {
                        Msg.Title = "File không đúng định dạng";
                        Msg.Error = true;
                        return Json(Msg);
                    }
                    var nvc = new NameValueCollection();
                    nvc.Add("project", uploadProject);
                    nvc.Add("UploadType", uploadType);
                    nvc.Add("StringDecypt", stringDecypt);
                    nvc.Add("EncryptType", "128");
                    nvc.Add("submit", submit);

                    var kq = string.Empty;
                    kq = FileStorage.HttpUploadFile(uploadUrl, file, "fileToUpload", file.ContentType, nvc);
                    if (!kq.Contains("OK"))
                    {
                        Msg.Error = true;
                        Msg.Title = "Upload file lỗi";
                    }
                }
                else
                {
                    Msg.Error = true;
                    Msg.Title = "Upload file lỗi";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Msg.Title = "Upload file lỗi";
                Msg.Error = true;
            }
            return Json(Msg);
        }
    }
}