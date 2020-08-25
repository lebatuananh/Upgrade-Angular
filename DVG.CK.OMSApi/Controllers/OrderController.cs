using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DVG.CK.OMSApi.Filter;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Order;
using DVG.WIS.Business.Product;
using DVG.WIS.Core.Constants;
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
    public class OrderController : BaseController
    {
        private IOrderBo _orderBo;
        private IDistrictBoCached _districtBoCached;
        private IWardBoCached _wardBoCached;
        private IProductBo _productBo;

        public OrderController(IOrderBo OrderBo, IDistrictBoCached districtBoCached, IWardBoCached wardBoCached, IProductBo productBo, IUserService userService, IHttpContextAccessor httpContextAccessor) : base(userService, httpContextAccessor)
        {
            _orderBo = OrderBo;
            _districtBoCached = districtBoCached;
            _wardBoCached = wardBoCached;
            _productBo = productBo;
        }


        [HttpGet]
        [Route("index")]
        [CustomizeAuthorize]
        public JsonResult Index()
        {
            var indexModel = new OrderIndexModel();
            indexModel.Role = _userService.GetRole();
            //indexModel.Status = status;
            var Msg = new Message();
            try
            {
                indexModel.ListOrderCountModel = _orderBo.CountOrder();
                Msg.Obj = indexModel;
            }
            catch (Exception ex)
            {
                Msg.Title = ex.Message;
                Msg.Error = true;
            }
            return AuthorizeJson(Msg);
        }
        [HttpGet]
        [Route("getcount")]
        [CustomizeAuthorizeAttribute]
        public JsonResult GetCount()
        {          
            var Msg = new Message();
            try
            {
                Msg.Obj = _orderBo.CountOrder();
            }
            catch (Exception ex)
            {
                Msg.Title = ex.Message;
                Msg.Error = true;
            }
            return AuthorizeJson(Msg);
        }
        [HttpPost]
        [Route("search")]
        [CustomizeAuthorize]
        public ActionResult Search(OrderSearchModel model)
        {
            var Msg = new Message();
            try
            {
                List<DVG.WIS.PublicModel.OrderOnListModel> listOrders = new List<DVG.WIS.PublicModel.OrderOnListModel>();
                int totalRow = 0;
                listOrders = _orderBo.GetList(model.KeyWord, model.Status, model.SourceType, model.DeliveryStatus, model.OrderType, model.FromCreatedDate, model.ToCreatedDate, model.RequestType, out totalRow, model.PageIndex, model.PageSize).ToList();
                List<District> listDistrict = _districtBoCached.GetAll().ToList();

                // nếu là màn hình bếp => lấy list Món ra
                var isDisplayListOrderDetails = (_userService.GetRole() == UserTypeEnum.Kitchen.GetHashCode() || _userService.GetRole() == UserTypeEnum.Checkfood.GetHashCode())
                    && (model.Status == OrderStatusEnum.Pending.GetHashCode() || model.Status == OrderStatusEnum.PushToPOS.GetHashCode() || model.Status == OrderStatusEnum.KitchenAccept.GetHashCode() || model.Status == OrderStatusEnum.KitchenDone.GetHashCode());

                if (listOrders.Any())
                {
                    foreach (var item in listOrders)
                    {
                        var district = listDistrict.Find(x => x.district_id == item.DistrictId);
                        if (district != null)
                        {
                            item.DistrictStr = district.district_name;
                        }

                        if (isDisplayListOrderDetails)
                        {
                            item.lstOrderDetail = _orderBo.GetByOrderId(item.OrderId);
                        }
                    }
                }
                var pager = new Pager { CurrentPage = model.PageIndex, PageSize = model.PageSize, TotalItem = totalRow };
                Msg.Obj = new { SearchModel = model, Data = listOrders, Pager = pager };
            }
            catch (Exception ex)
            {
                Msg.Obj = null;
                Msg.Title = ex.Message;
                Msg.Error = true;
            }

            return AuthorizeJson(Msg);
        }

        [HttpGet]
        [Route("update")]
        [CustomizeAuthorize(AdminRole, CustomerServiceRole)]
        public ActionResult Update(string encryptId)
        {
            OrderFullViewModel model = new OrderFullViewModel();
            int id = !string.IsNullOrEmpty(encryptId) ? EncryptUtility.DecryptId(encryptId) : 0;
            //List<EnumInfo> listOrderType = GetEnumValuesAndDescriptions<OrderTypeEnum>();
            //List<EnumInfo> listSourceType = GetEnumValuesAndDescriptions<SourceTypeEnum>();
            List<OrderDetailViewModel> listOrderDetail = new List<OrderDetailViewModel>();
            int totalRow = 0;
            List<Product> listProduct = (List<Product>)_productBo.GetList("", out totalRow);
            if (listProduct != null && listProduct.Count > 0)
            {
                foreach (Product product in listProduct)
                {
                    OrderDetailViewModel orderDetailViewModel = new OrderDetailViewModel(product);
                    listOrderDetail.Add(orderDetailViewModel);
                }
            }
            if (id > 0)
            {
                model = _orderBo.GetByFullValueById(id);
                model.lstOrderDetailViewModel = listOrderDetail;
                model.ListProductViewModel = _orderBo.GetByOrderId(id).ToList();
                model.ListDistrict = (List<District>)_districtBoCached.GetAll();
                model.ListWard = (List<Ward>)_wardBoCached.GetByDistrictId(model.DistrictId);
                if(model.OrderType == 1) //--Lấy trực tiếp
                {
                    model.TabIndex = 3;
                }
                else
                {
                    if (model.DeliverDate == DateTime.MinValue)
                    {
                        model.DeliverDate = DateTime.Now;
                        model.TabIndex = 1;
                    }
                    else
                    {
                        model.TabIndex = 2;
                    }
                }
                
                //model.DeliverDateStr = model.DeliverDate.ToString("dd/MM/yyyy HH:mm");

                ViewBag.Title = "Sửa đơn hàng";
            }
            else
            {
                var result = new Order();
                List<District> listDistrict = (List<District>)_districtBoCached.GetAll();
                List<Ward> listWard = new List<Ward>();

                model = new DVG.WIS.PublicModel.OrderFullViewModel(result, new District(), new Ward(), listOrderDetail);
                model.ListDistrict = listDistrict;
                //model.ListOrderType = listOrderType;
                model.DeliverDate = DateTime.Now;
                model.DeliverDateStr = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                model.ListProductViewModel = new List<OrderDetailViewModel>();
                model.OrderType = -1;
                model.SourceType = -1;

                ViewBag.Title = "Tạo đơn hàng";
                model.TabIndex = 1;
            }
            model.ProductViewModelItem = new OrderDetailViewModel();
            model.CityCode = "SG";
            Msg.Obj = model;
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("updateorder")]
        [CustomizeAuthorize(AdminRole, CustomerServiceRole)]
        public ActionResult UpdateOrder(OrderFullViewModel model)
        {

            if (model.Status == (short)OrderStatusEnum.Inactive || model.Status == (short)OrderStatusEnum.Pending)
            {
                ErrorCodes error = _orderBo.CreateOrder(model);
                if ((int)error == (int)ErrorCodes.Success)
                {
                    Msg.Error = false;
                }
                else
                {
                    Msg.Error = true;
                    Msg.Title = GetEnumDescription(error);
                }
            }
            else
            {
                Msg.Error = true;
                Msg.Title = "Không thể update đơn hàng này!";
            }

            return AuthorizeJson(Msg);
        }

        public static List<EnumInfo> GetEnumValuesAndDescriptions<T>()
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T is not System.Enum");

            List<EnumInfo> enumValList = new List<EnumInfo>();

            foreach (var e in Enum.GetValues(typeof(T)))
            {
                var fi = e.GetType().GetField(e.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                enumValList.Add(new EnumInfo(Convert.ToInt16((int)e), e.ToString(), (attributes.Length > 0) ? attributes[0].Description : e.ToString()));
            }

            return enumValList;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        [HttpPost]
        [Route("aproveorder")]
        [CustomizeAuthorize(AdminRole, CustomerServiceRole)]
        public JsonResult AproveOrder(int orderId)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.UpdateStatus(orderId, OrderStatusEnum.Pending, user: user);
                Msg.Error = errorCode != ErrorCodes.Success;
            }
            Msg.Title = StringUtils.GetEnumDescription(errorCode);
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("kitchenaccept")]
        [CustomizeAuthorize(AdminRole, KitchenRole)]
        public JsonResult KitchenAccept(int orderId)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.UpdateStatus(orderId, OrderStatusEnum.KitchenAccept, user: user);
                Msg.Error = errorCode != ErrorCodes.Success;
            }
            Msg.Title = StringUtils.GetEnumDescription(errorCode);
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("kitchendone")]
        [CustomizeAuthorize(AdminRole, CheckfoodRole)]
        public JsonResult KitchenDone(int orderId)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.UpdateStatus(orderId, OrderStatusEnum.KitchenDone, user: user);
                Msg.Error = errorCode != ErrorCodes.Success;
            }
            Msg.Title = StringUtils.GetEnumDescription(errorCode);
            return AuthorizeJson(responseData);
        }

        [HttpPost]
        [Route("cashierreceive")]
        [CustomizeAuthorize(AdminRole, CashierRole)]
        public JsonResult CashierReceive(int orderId)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            if (orderId > 0)
            {
                var user = _userService.GetUserName();

                errorCode = _orderBo.UpdateStatus(orderId, OrderStatusEnum.Delivering, user: user);
                Msg.Error = errorCode != ErrorCodes.Success;
            }
            Msg.Title = StringUtils.GetEnumDescription(errorCode);
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("deliverytoordercomplete")]
        [CustomizeAuthorize(AdminRole, CashierRole, CustomerServiceRole, KitchenManagerRole)]
        public JsonResult DeliveryToOrderComplete(int orderId)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.UpdateStatus(orderId, OrderStatusEnum.Success, user: user);
                Msg.Error = errorCode != ErrorCodes.Success;
            }
            Msg.Title = StringUtils.GetEnumDescription(errorCode);
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("failuretoordercomplete")]
        [CustomizeAuthorize(AdminRole, KitchenManagerRole)]
        public JsonResult FailureToOrderComplete(int orderId)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.UpdateStatus(orderId, OrderStatusEnum.Success, user: user);
                Msg.Error = errorCode != ErrorCodes.Success;
            }
            Msg.Title = StringUtils.GetEnumDescription(errorCode);
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("requestdestroy")]
        [CustomizeAuthorize(AdminRole, CustomerServiceRole)]
        public JsonResult RequestDestroy(int orderId, string reasonNote)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.UpdateRequestType(orderId, RequestTypeEnum.CSRequestDestroy, reasonNote: reasonNote, user: user);
                Msg.Error = errorCode != ErrorCodes.Success;
            }
            Msg.Title = StringUtils.GetEnumDescription(errorCode);
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("requestconfirmcustomer")]
        [CustomizeAuthorize(AdminRole, CustomerServiceRole)]
        public JsonResult RequestConfirmCustomer(int orderId)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.UpdateRequestType(orderId, RequestTypeEnum.ConfirmCustomerForDestroy, user: user);
                Msg.Error = errorCode != ErrorCodes.Success;
            }
            Msg.Title = StringUtils.GetEnumDescription(errorCode);
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("destroy")]
        [CustomizeAuthorize(AdminRole, KitchenManagerRole)]
        public JsonResult Destroy(int orderId, string reasonNote)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                var cashier = _userService.GetCurrentCashier();

                // TH nếu là CS hủy đơn thì phải là đơn chờ xác nhận
                var isCS = _userService.GetRole() == UserTypeEnum.CustomerService.GetHashCode();

                errorCode = _orderBo.UpdateStatus(orderId, OrderStatusEnum.Destroy, reasonNote: reasonNote, user: user, cashierRefund: cashier, isCS: isCS);
                Msg.Error = errorCode != ErrorCodes.Success;
            }
            Msg.Title = StringUtils.GetEnumDescription(errorCode);
            return AuthorizeJson(Msg);
        }

        [CustomizeAuthorize]
        [Route("getorderbyid")]
        public JsonResult GetOrderById(int orderId)
        {
            ResponseData responseData = new ResponseData();

            try
            {
                if (orderId > 0)
                {
                    var model = _orderBo.GetById(orderId);
                    Msg.Error = false;
                    Msg.Obj = model;
                }
            }
            catch (Exception ex)
            {
                Msg.Error = false;
                Msg.Obj = new WIS.PublicModel.OrderViewModel();
            }

            return AuthorizeJson(Msg);
        }

        [HttpGet]
        [Route("printkitchen")]
        public ActionResult PrintKitchen(int orderId)
        {
            if (orderId > 0)
            {
                var order = _orderBo.GetById(orderId);
                return View("PrintKitchen", order);
            }
            return View();
        }

        [HttpGet]
        [Route("printcashier")]
        public ActionResult PrintCashier(int orderId)
        {
            if (orderId > 0)
            {
                var order = _orderBo.GetById(orderId);
                return View("PrintCashier", order);
            }
            return View();
        }

        [HttpPost]
        [Route("callprinter")]
        [CustomizeAuthorize]
        public JsonResult CallPrinter(int orderId, int type)
        {

            try
            {
                if (StaticVariable.EnablePrint)
                {
                    switch (type)
                    {
                        case 1:
                            {
                                var content = string.Empty;
                                HttpStatusCode statusCode;
                                HttpRequestUtils.PostRequest(StaticVariable.PrintKitchenApi + orderId, string.Empty, out statusCode, out content);
                                Msg.Error = false;
                                break;
                            }
                        case 2:
                            {
                                var content = string.Empty;
                                HttpStatusCode statusCode;
                                HttpRequestUtils.PostRequest(StaticVariable.PrintCashierApi + orderId, string.Empty, out statusCode, out content);
                                Msg.Error = false;
                                break;
                            }
                    }
                }
                else
                {
                    Msg.Error = false;
                }
            }
            catch (Exception ex)
            {
                Msg.Title = "Lỗi không xác định";
                Msg.Error = true;
            }

            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("callahamoveshipping")]
        [CustomizeAuthorize(AdminRole, CashierRole)]
        public JsonResult CallAhamoveShipping(int orderId)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.ProcessCallAhamove(orderId, user);
                Msg.Error = errorCode != ErrorCodes.Success;
            }
            Msg.Title = StringUtils.GetEnumDescription(errorCode);
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("callanothershipping")]
        [CustomizeAuthorize(AdminRole, CashierRole)]
        public JsonResult CallAnotherShipping(int orderId, int shipFee)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.UpdateDeliveryPrice(orderId, user, shipFee);

                Msg.Error = errorCode != ErrorCodes.Success;
            }
            Msg.Title = StringUtils.GetEnumDescription(errorCode);
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [CustomizeAuthorize]
        [Route("getsuggestaddress")]
        public JsonResult GetSuggestAddress(string keyword)
        {
           
            try
            {
                var model = _orderBo.GetSuggestAddress(keyword);
                Msg.Error = false;
                Msg.Obj = model;
            }
            catch (Exception ex)
            {
                Msg.Error = false;
                Msg.Obj = null;
            }

            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [CustomizeAuthorize]
        [Route("searchcustomerbyphone")]
        public JsonResult SearchCustomerByPhone(string keyword)
        {

            try
            {
                var model = _orderBo.SearchCustomerByPhone(keyword);
                Msg.Error = false;
                Msg.Obj = model;
            }
            catch (Exception ex)
            {
                Msg.Error = false;
                Msg.Obj = null;
            }

            return AuthorizeJson(Msg);
        }
    }
}
