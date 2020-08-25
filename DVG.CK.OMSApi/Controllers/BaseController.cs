using DVG.WIS.Business.Authenticator;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DVG.CK.OMSApi.Controllers
{
    public class BaseController : Controller
    {
        public const int AdminRole = (int)UserTypeEnum.Admin;
        public const int CashierRole = (int)UserTypeEnum.Cashier;
        public const int CustomerServiceRole = (int)UserTypeEnum.CustomerService;
        public const int KitchenRole = (int)UserTypeEnum.Kitchen;
        public const int KitchenManagerRole = (int)UserTypeEnum.KitchenManager;
        public const int CheckfoodRole = (int)UserTypeEnum.Checkfood;

        protected IHttpContextAccessor _httpContextAccessor;
        public static Message Msg;
        protected IUserService _userService;

        public BaseController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
            Msg = new Message();
            _userService = userService;
        }

        protected JsonResult AuthorizeJson<T>(T content)
        {
            if (content != null && _httpContextAccessor.HttpContext != null)
            {
                var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                var tokenObj = identity.Claims.Where(x => x.Type == ClaimTypes.Authentication).FirstOrDefault();
                var checksumObj = identity.Claims.Where(x => x.Type == ClaimTypes.Sid).FirstOrDefault();
                if (tokenObj != null && !string.IsNullOrEmpty(tokenObj.Value)
                    && checksumObj != null && !string.IsNullOrEmpty(checksumObj.Value))
                {
                    // check thêm nếu có refresh token thì phải trùng trong cache, không thì là bị đúp
                    if (_userService.CheckExitstJWTTokenOnCache(checksumObj.Value, tokenObj.Value))
                    {
                        var entityType = content.GetType();
                        var refreshToken = entityType.GetProperty("RefreshToken");
                        if (refreshToken != null)
                        {
                            refreshToken.SetValue(content, true);

                            var token = entityType.GetProperty("Token");
                            if (token != null)
                            {
                                token.SetValue(content, tokenObj.Value);
                            }
                        }
                    }
                }
            }
            return Json(content);
        }
    }
}
