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
    public class StatisticController : BaseController
    {
        private IOrderBo _orderBo;

        public StatisticController(IOrderBo OrderBo, IUserService userService, IHttpContextAccessor httpContextAccessor) : base(userService, httpContextAccessor)
        {
            _orderBo = OrderBo;
        }

        [CustomizeAuthorizeAttribute(AdminRole, KitchenManagerRole)]
        [Route("index")]
        public JsonResult Index()
        {
            Msg.Obj = new CashierRevenueIndexModel();
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("getcashierrevenuestatistic")]
        [CustomizeAuthorizeAttribute(AdminRole, KitchenManagerRole)]
        public JsonResult GetCashierRevenueStatistic(CashierRevenueSearchModel model)
        {
            try
            {
                //if (model.FromDate == default(DateTime))
                //    model.FromDate = DateTime.Now;
                //if (model.ToDate == default(DateTime))
                //    model.ToDate = DateTime.Now;
                if (!string.IsNullOrEmpty(model.FromDateStr))
                {
                    model.FromDate = System.DateTime.Parse(model.FromDateStr);
                }
                if (!string.IsNullOrEmpty(model.ToDateStr))
                {
                    model.ToDate = System.DateTime.Parse(model.ToDateStr);
                }
                var lstDate = new List<DateTime>();
                model.LstData = _orderBo.GetCashierRevenueStatistic(model.FromDate, model.ToDate, model.Type, out lstDate);
                model.LstDate = lstDate;
                model.LstDataSouceType = _orderBo.GetCashierRevenueStatisticBySourceType(model.FromDate, model.ToDate, model.Type);
                model.LstDataPaymentType = _orderBo.GetCashierRevenueStatisticByPaymentType(model.FromDate, model.ToDate, model.Type);
                model.LstDataProduct = _orderBo.GetCashierRevenueStatisticByProduct(model.FromDate, model.ToDate);
                model.LstCountProduct = _orderBo.CountCashierRevenueStatisticByProduct(model.FromDate, model.ToDate);
                model.LstCountBill = _orderBo.CountOrderAndCustomer(model.FromDate, model.ToDate);
                Msg.Error = false;
                Msg.Obj = model;

            }
            catch (Exception ex)
            {
                Msg.Error = true;
                Msg.Obj = new WIS.PublicModel.OrderViewModel();
            }
            return AuthorizeJson(Msg);
        }
    }
}
