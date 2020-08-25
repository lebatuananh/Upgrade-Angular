using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DVG.CK.OMS.Models;
using Microsoft.AspNetCore.Authorization;
using DVG.WIS.DAL.Ward;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;

namespace DVG.CK.OMS.Controllers
{

    public class WardController : Controller
    {
        private IWardBoCached _wardBoCached;
        public WardController(IWardBoCached wardBoCached)
        {
            _wardBoCached = wardBoCached;
        }
        [HttpPost]
        public ActionResult SearchByDistrictId(int districtId)
        {
            List<Ward> list = (List<Ward>)_wardBoCached.GetByDistrictId(districtId);
            ResponseData response = new ResponseData();
            response.Success = true;
            response.Data = list;
            return Json(response);
        }
    }
}
