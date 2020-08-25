using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using DVG.WIS.Business.Account;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Core;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DVG.CK.OMS.Controllers
{
    public class AccountController : Controller
    {
        private IAccountBo _accountBo;
        private IUserService _userService;

        public AccountController(IAccountBo accountBo, IUserService userService)
        {
            this._accountBo = accountBo;
            this._userService = userService;
        }

        public ActionResult Login(string returnUrl = "")

        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/Home";
            }
            if (_userService.IsLogin())
            {
                return Redirect(returnUrl);
            }

            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Request.Path;
            HttpContext.Session.Set(Const.SessionCurrentUrl, returnUrl);

            ViewBag.ReturnUrl = returnUrl;

            if (AppSettings.Instance.GetBool(Const.UsingSSO))
            {
                return Redirect(AppSettings.Instance.GetString(Const.SSOLogin));
            }

            return View();
        }

        [HttpPost]
        public ActionResult DoLogin(string email, string password, bool isSavedPassword = false)
        {
            var loginResult = _userService.LoginForCms(email, password, isSavedPassword);
            return Json(new { status = loginResult.Success, message = loginResult.Message });
        }

        public ActionResult Logout()
        {
            string returnUrl = string.Empty;

            if (HttpContext.Request.Headers["Referer"].ToString() != null)
            {
                returnUrl = HttpContext.Request.Headers["Referer"].ToString();
                if (returnUrl.Contains("LogOnSSO"))
                {
                    returnUrl = string.Empty;
                }
            }

            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = "/Home";

            _userService.Logout();

            if (AppSettings.Instance.GetBool(Const.UsingSSO))
            {
                returnUrl = string.Format("{0}?returnUrl={1}", AppSettings.Instance.GetString(Const.SSOLogoutCallback),
                    HttpUtility.UrlEncode(returnUrl));
            }

            return Redirect(returnUrl);
        }

        [Authorize]
        public ActionResult Manager()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public JsonResult ChangePassword(ChangePasswordModel saveModel)
        {
            string message = string.Empty;
            if (string.IsNullOrEmpty(saveModel.CurrentPassword))
            {
                message = "Mật khẩu hiện tại không đúng";
            }
            else if (string.IsNullOrEmpty(saveModel.Password))
            {
                message = "Mật khẩu không hợp lệ";
            }
            else if (string.IsNullOrEmpty(saveModel.ConfirmPassword))
            {
                message = "Mật khẩu xác nhận không đúng";
            }
            else
            {
                var userInfo = _userService.GetUserLogin();
                var errors = _accountBo.ChangePassword(userInfo.UserName, saveModel.CurrentPassword, saveModel.Password,
                    saveModel.ConfirmPassword);
                message = StringUtils.GetEnumDescription(errors);
                if (errors == ErrorCodes.Success)
                {
                    _userService.Logout();
                    return Json(new { status = true, message });
                }
            }
            return Json(new { status = false, message });
        }

        [Authorize]
        public ActionResult AccountInfo()
        {
            UserLogin userLogin = _userService.GetUserLogin();
            return View(userLogin);
        }

        public ActionResult PermissionDenied()
        {
            return View();
        }
    }
}