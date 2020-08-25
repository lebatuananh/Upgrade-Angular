using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DVG.CK.OMS.Controllers
{
    public class OrderController : Controller
    {
        private IOrderBo _orderBo;
        private IDistrictBoCached _districtBoCached;
        private IWardBoCached _wardBoCached;
        private IProductBo _productBo;
        private IUserService _userService;

        public OrderController(IOrderBo OrderBo, IDistrictBoCached districtBoCached, IWardBoCached wardBoCached, IProductBo productBo, IUserService userService)
        {
            _orderBo = OrderBo;
            _districtBoCached = districtBoCached;
            _wardBoCached = wardBoCached;
            _productBo = productBo;
            _userService = userService;
        }

        [CustomAuthorizeAttribute]
        public ActionResult Index(short status = -1)
        {
            var model = new OrderIndexModel();
            model.Role = _userService.GetRole();
            model.Status = status;
            return View(model);
        }

        [HttpPost]
        [CustomAuthorizeAttribute]
        public ActionResult Search(OrderSearchModel model)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                List<DVG.WIS.PublicModel.OrderOnListModel> listOrders = new List<DVG.WIS.PublicModel.OrderOnListModel>();
                int totalRow = 0;
                listOrders = _orderBo.GetList(model.KeyWord, model.Status, model.SourceType, model.DeliveryStatus, model.OrderType, model.FromCreatedDate, model.ToCreatedDate, model.RequestType, out totalRow, model.PageIndex, model.PageSize).ToList();
                List<District> listDistrict = (List<District>)_districtBoCached.GetAll();

                // nếu là màn hình bếp => lấy list Món ra
                var isDisplayListOrderDetails = _userService.GetRole() == UserTypeEnum.Kitchen.GetHashCode()
                    && (model.Status == OrderStatusEnum.PushToPOS.GetHashCode() || model.Status == OrderStatusEnum.KitchenAccept.GetHashCode());

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
                responseData.Data = listOrders;
                responseData.TotalRow = totalRow;
                responseData.Success = true;
            }
            catch (Exception ex)
            {
                responseData.Data = null;
                responseData.Success = false;
            }

            return Json(responseData);
        }


        [CustomAuthorizeAttribute(Policy = "CustomerServiceRole")]
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
                if (model.DeliverDate == DateTime.MinValue)
                {
                    model.DeliverDate = DateTime.Now;
                }
                else
                {

                }
                model.DeliverDateStr = model.DeliverDate.ToString("dd/MM/yyyy HH:mm");

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
            }
            model.ProductViewModelItem = new OrderDetailViewModel();
            model.CityCode = "SG";

            return View(model);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Policy = "CustomerServiceRole")]
        public ActionResult UpdateOrder(OrderFullViewModel model)
        {
            ResponseData responseData = new ResponseData();
            if (model.Status == (short)OrderStatusEnum.Inactive || model.Status == (short)OrderStatusEnum.Pending)
            {
                ErrorCodes error = _orderBo.CreateOrder(model);
                if ((int)error == (int)ErrorCodes.Success)
                {
                    responseData.Success = true;
                }
                else
                {
                    responseData.Success = false;
                    responseData.Message = GetEnumDescription(error);
                }
            }
            else
            {
                responseData.Success = false;
                responseData.Message = "Không thể update đơn hàng này!";
            }

            return Json(responseData);
        }

        [CustomAuthorizeAttribute]
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

        [CustomAuthorizeAttribute]
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
        [CustomAuthorizeAttribute(Policy = "CustomerServiceRole")]
        public JsonResult AproveOrder(int orderId)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.UpdateStatus(orderId, OrderStatusEnum.Pending, user: user);
                responseData.Success = errorCode == ErrorCodes.Success;
            }
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            return Json(responseData);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Policy = "KitchenRole")]
        public JsonResult KitchenAccept(int orderId)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.UpdateStatus(orderId, OrderStatusEnum.KitchenAccept, user: user);
                responseData.Success = errorCode == ErrorCodes.Success;
            }
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            return Json(responseData);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Policy = "KitchenRole")]
        public JsonResult KitchenDone(int orderId)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.UpdateStatus(orderId, OrderStatusEnum.KitchenDone, user: user);
                responseData.Success = errorCode == ErrorCodes.Success;
            }
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            return Json(responseData);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Policy = "CashierRole")]
        public JsonResult CashierReceive(int orderId)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                
                errorCode = _orderBo.UpdateStatus(orderId, OrderStatusEnum.Delivering, user: user);
                responseData.Success = errorCode == ErrorCodes.Success;
            }
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            return Json(responseData);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Policy = "CustomerServiceRole")]
        public JsonResult RequestDestroy(int orderId, string reasonNote)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.UpdateRequestType(orderId, RequestTypeEnum.CSRequestDestroy, reasonNote: reasonNote, user: user);
                responseData.Success = errorCode == ErrorCodes.Success;
            }
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            return Json(responseData);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Policy = "CustomerServiceRole")]
        public JsonResult RequestConfirmCustomer(int orderId)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                errorCode = _orderBo.UpdateRequestType(orderId, RequestTypeEnum.ConfirmCustomerForDestroy, user: user);
                responseData.Success = errorCode == ErrorCodes.Success;
            }
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            return Json(responseData);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Policy = "DestroyOrderRole")]
        public JsonResult Destroy(int orderId, string reasonNote)
        {
            ErrorCodes errorCode = ErrorCodes.UnknowError;
            ResponseData responseData = new ResponseData();
            if (orderId > 0)
            {
                var user = _userService.GetUserName();
                var cashier = _userService.GetCurrentCashier();

                // TH nếu là CS hủy đơn thì phải là đơn chờ xác nhận
                var isCS = _userService.GetRole() == UserTypeEnum.CustomerService.GetHashCode();

                errorCode = _orderBo.UpdateStatus(orderId, OrderStatusEnum.Destroy, reasonNote: reasonNote, user: user, cashierRefund: cashier, isCS: isCS);
                responseData.Success = errorCode == ErrorCodes.Success;
            }
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            return Json(responseData);
        }

        [CustomAuthorizeAttribute]
        public JsonResult GetOrderById(int orderId)
        {
            ResponseData responseData = new ResponseData();

            try
            {
                if (orderId > 0)
                {
                    var model = _orderBo.GetById(orderId);
                    responseData.Success = true;
                    responseData.Data = model;
                }
            }
            catch (Exception ex)
            {
                responseData.Success = false;
                responseData.Data = new WIS.PublicModel.OrderViewModel();
            }

            return Json(responseData);
        }

        [HttpGet]
        public ActionResult PrintKitchen(int orderId)
        {
            if (orderId > 0)
            {
                var order = _orderBo.GetById(orderId);
                return View(order);
            }
            return null;
        }

        [HttpGet]
        public ActionResult PrintCashier(int orderId)
        {
            if (orderId > 0)
            {
                var order = _orderBo.GetById(orderId);
                return View(order);
            }
            return null;
        }

        [HttpPost]
        [CustomAuthorizeAttribute]
        public JsonResult CallPrinter(int orderId, int type)
        {
            ResponseData responseData = new ResponseData();

            try
            {
                switch (type)
                {
                    case 1:
                        {
                            var content = string.Empty;
                            HttpStatusCode statusCode;
                            HttpRequestUtils.PostRequest(StaticVariable.PrintKitchenApi + orderId, string.Empty, out statusCode, out content);
                            responseData.Success = true;
                            break;
                        }
                    case 2:
                        {
                            var content = string.Empty;
                            HttpStatusCode statusCode;
                            HttpRequestUtils.PostRequest(StaticVariable.PrintCashierApi + orderId, string.Empty, out statusCode, out content);
                            responseData.Success = true;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                responseData.Message = "Lỗi không xác định";
                responseData.Success = false;
            }

            return Json(responseData);
        }
    }
}