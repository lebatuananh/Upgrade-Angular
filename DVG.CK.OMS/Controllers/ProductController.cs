using System;
using System.Linq;
using DVG.WIS.Business.Product;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace DVG.CK.OMS.Controllers
{
    [CustomAuthorizeAttribute]
    public class ProductController : Controller
    {
        private IProductBo _productBo;

        public ProductController(IProductBo productBo)
        {
            _productBo = productBo;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Search(ProductSearchModel model)
        {
            ResponseData responseData = new ResponseData();
            responseData.Success = false;
            try
            {
                int totalRow = 0;
                var lstProduct = _productBo.GetList(model.KeyWord, out totalRow, model.PageIndex, model.PageSize).ToList();
                if (lstProduct.Any())
                {
                    var lstModel = lstProduct.Select(x => new ProductViewModel(x));
                    responseData.Data = lstModel;
                    responseData.TotalRow = totalRow;
                    responseData.Success = true;
                }
            }
            catch (Exception ex)
            {
                responseData.Data = null;
            }

            return Json(responseData);
        }
    }
}
