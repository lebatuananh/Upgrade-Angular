using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileManager.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DVG.CK.OMS.FileManager
{
    public class IndexModel : PageModel
    {
        public string UploadProject { get; set; }
        public string UrlUploadFile { get; set; }
        public string Token { get; set; }
        public void OnGet()
        {
            string userName = "test"; //HttpContext.Current.User.Identity.Name;
            FileStorage.AESIV = Config.AESIV;
            FileStorage.AESKey = Config.AESKey;

            Token = FileStorage.AESEncrypt(userName + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            UploadProject = Config.UploadProject;
            UrlUploadFile = Config.UploadDomain + Config.UploadHandler;
        }
    }
}