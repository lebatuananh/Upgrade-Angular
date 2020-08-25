using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVG.CK.OMSApi.Filter;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Base;
using DVG.WIS.Business.Notify;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DVG.CK.OMSApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotifyController : BaseController
    {
        private INotifyService _notifyService;
        public NotifyController(INotifyService notifyService, IUserService userService, IHttpContextAccessor httpContextAccessor) : base(userService, httpContextAccessor)
        {
            _notifyService = notifyService;
        }

        [HttpPost]
        [Route("getnotifyaccount")]
        [CustomizeAuthorizeAttribute]
        public JsonResult GetNotifyAccount()
        {
            try
            {
                var result = _notifyService.GetNotifyForAccounts(_userService.GetUserName());
                Msg.Obj = result;
                Msg.Error = false;
            }
            catch (Exception ex)
            {
                Msg.Error = true;
            }
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("deletenotifyaccount")]
        [CustomizeAuthorizeAttribute]
        public JsonResult DeleteNotifyAccount()
        {
            try
            {
                var result = _notifyService.DeleteNotifyAccount(_userService.GetUserName());
                Msg.Obj = result;
                Msg.Error = false;
            }
            catch (Exception ex)
            {
                Msg.Error = true;
            }
            return AuthorizeJson(Msg);
        }
    }
}
