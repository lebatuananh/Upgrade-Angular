using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using DVG.CK.OMSApi.Controllers;
using DVG.CK.OMSApi.Filter;
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
    [Route("[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private IAccountBo _accountBo;

        public AccountController(IAccountBo accountBo, IUserService userService, IHttpContextAccessor httpContextAccessor) : base(userService, httpContextAccessor)
        {
            this._accountBo = accountBo;
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
        [Route("dologin")]
        public ActionResult DoLogin(SystemUserLoginModel user)
        {
            var loginResult = _userService.LoginForCms(user.UserName, user.Password, false);
            return Json(loginResult);
        }

        [Route("DoLogout")]
        [CustomizeAuthorize]
        public ActionResult Logout()
        {
            var result = new ResponseData();
            result = _userService.Logout();
            return Json(result);
        }

        public ActionResult Manager()
        {
            return View();
        }

        [HttpPost]
        [Route("changepassword")]
        [CustomizeAuthorize]
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