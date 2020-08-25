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
    public class ProductController : BaseController
    {
        private IProductBo _productBo;

        public ProductController(IProductBo productBo, IUserService userService, IHttpContextAccessor httpContextAccessor) : base(userService, httpContextAccessor)
        {
            _productBo = productBo;
        }

        [Route("search")]
        [HttpPost]
        [CustomizeAuthorizeAttribute]
        public ActionResult Search(ProductSearchModel model)
        {            
            try
            {
                int totalRow = 0;
                var lstProduct = _productBo.GetList(model.KeyWord, out totalRow, model.PageIndex, model.PageSize).ToList();
                if (lstProduct.Any())
                {
                    var lstModel = lstProduct.Select(x => new ProductViewModel(x));
                    var pager = new Pager { CurrentPage = model.PageIndex, PageSize = model.PageSize, TotalItem = totalRow };
                    Msg.Obj = new { Data = lstModel, Pager = pager };

                    Msg.Error = false;
                }
                else
                {
                    Msg.Obj = new { Data = lstProduct, Pager = new Pager() };
                }
            }
            catch (Exception ex)
            {
                Msg.Obj = null;
                Msg.Error = true;
            }

            return AuthorizeJson(Msg);
        }
    }
}
