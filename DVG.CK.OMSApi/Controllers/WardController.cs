using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DVG.CK.OMSApi.Filter;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Order;
using DVG.WIS.Business.Product;
using DVG.WIS.Core.Enums;
using DVG.WIS.DAL.District;
using DVG.WIS.DAL.Ward;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DVG.CK.OMSApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WardController : BaseController
    {
        private IWardBoCached _wardBoCached;
        public WardController(IWardBoCached wardBoCached, IUserService userService, IHttpContextAccessor httpContextAccessor) : base(userService, httpContextAccessor)
        {
            _wardBoCached = wardBoCached;
        }
        [Route("searchbydistrictid")]
        [HttpPost]
        [CustomizeAuthorizeAttribute]
        public ActionResult SearchByDistrictId(int districtId)
        {
            List<Ward> list = (List<Ward>)_wardBoCached.GetByDistrictId(districtId);
            ResponseData response = new ResponseData();
            Msg.Error = false;
            Msg.Obj = list;
            return AuthorizeJson(Msg);
        }
    }
}
