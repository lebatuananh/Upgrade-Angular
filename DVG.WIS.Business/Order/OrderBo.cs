using DVG.WIS.Business.AhaMove;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Notify;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.DAL.District;
using DVG.WIS.DAL.Order;
using DVG.WIS.DAL.OrderDetail;
using DVG.WIS.DAL.OrderHistory;
using DVG.WIS.DAL.ThirdPartyLog;
using DVG.WIS.DAL.Ward;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using DVG.WIS.PublicModel.AhaMove;
using DVG.WIS.Utilities;
using DVG.WIS.Utilities.Logs;
using DVG.WIS.Utilities.Serialization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Text;
using System.Web;

namespace DVG.WIS.Business.Order
{
    public class OrderBo : IOrderBo
    {
        private IOrderDal _orderDal;
        private IOrderDetailDal _orderDetailDal;
        private IOrderHistoryBo _orderHistoryBo;
        private IThirdPartyLogBo _thirdPartyLogBo;
        private IDistrictBoCached _districtBoCached;
        private IWardBoCached _wardBoCached;
        private IUserService _userService;
        private INotifyService _notifyService;

        public OrderBo(IOrderDal orderDal, IOrderDetailDal orderDetailDal, IOrderHistoryBo orderHistoryBo, IThirdPartyLogBo thirdPartyLogBo, IDistrictBoCached districtBoCached, IWardBoCached wardBoCached, IUserService userService, INotifyService notifyService)
        {
            this._orderDal = orderDal;
            this._orderDetailDal = orderDetailDal;
            this._orderHistoryBo = orderHistoryBo;
            this._thirdPartyLogBo = thirdPartyLogBo;
            this._districtBoCached = districtBoCached;
            this._wardBoCached = wardBoCached;
            this._userService = userService;
            this._notifyService = notifyService;
        }

        public ErrorCodes CreateOrder(OrderFullViewModel model)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                var user = _userService.GetUserLogin().UserName;
                model.DeliverDate = DateTime.ParseExact(model.DeliverDateStr, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                if (model.Code == Guid.Empty)
                    model.Code = Guid.NewGuid();
                ///------- bỏ Code thay bằng OrderCode
                if (string.IsNullOrEmpty(model.OrderCode))
                    model.OrderCode = StringUtils.GenerateUniqueId();
                model.CreatedBy = user;
                model.ModifiedBy = user;
                model.CustomerNote = string.IsNullOrEmpty(model.CustomerNote) ? string.Empty : model.CustomerNote;
                model.OrderPrice = int.Parse(model.OrderPriceStr.Replace(".", ""));
                model.OrderOriginPrice = int.Parse(model.OrderOriginPriceStr.Replace(".", ""));
                var result = 0;
                var change_log = "";
                if (model.OrderId == 0)
                {
                    result = _orderDal.CreateOrder(model);
                    change_log = "Tạo đơn hàng";
                }
                else
                {
                    result = _orderDal.UpdateOrder(model);
                    change_log = "Sửa đơn hàng";
                }
                if (result <= 0) errorCodes = ErrorCodes.BusinessError;
                else
                {
                    _orderHistoryBo.Insert(new OrderHistory
                    {
                        change_log = change_log,
                        created_by = user,
                        order_id = result,
                        status = model.Status
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                errorCodes = ErrorCodes.Exception;
            }
            return errorCodes;
        }

        public ErrorCodes UpdateStatus(int orderId, OrderStatusEnum changeStatus, string reasonNote = "", string user = "", string cashierRefund = "", bool isCS = false)
        {
            ErrorCodes errorCodes = ErrorCodes.UnknowError;
            try
            {
                var result = 0;
                var order = _orderDal.GetById(orderId);
                if (order != null && order.order_id > 0)
                {
                    var currentStatus = (OrderStatusEnum)order.status;

                    // Nếu là đơn Take Away thì từ KitchenDone => Success luôn
                    if (currentStatus == OrderStatusEnum.KitchenDone && changeStatus == OrderStatusEnum.Delivering && order.order_type == OrderTypeEnum.TakeAway.GetHashCode())
                        changeStatus = OrderStatusEnum.Success;

                    if (!ValidateChangeStatus(currentStatus, changeStatus))
                    {
                        return ErrorCodes.StatusError;
                    }

                    // Nếu là CS hủy đơn thì đơn phải là trạng thái chờ xác nhận
                    if (isCS && changeStatus == OrderStatusEnum.Destroy && currentStatus != OrderStatusEnum.Inactive)
                    {
                        return ErrorCodes.StatusError;
                    }

                    var changeStatusShort = (short)changeStatus;
                    var changeRequestType = (short)order.request_type;

                    switch (changeStatus)
                    {
                        #region TH: Bếp báo done, service call để đẩy sang Ahamove. Sau khi Shipper đến nhận hàng giao tiền thì thu ngân click nhận tiền
                        case OrderStatusEnum.Delivering:
                            result = _orderDal.UpdateStatus(orderId, changeStatusShort, changeRequestType, user, reasonNote: order.reason_note, cashierReceive: user);
                            break;
                        #endregion
                        #region TH: Quản lý hủy đơn mà do giao hàng không thanh công, phải refund tiền cho shipper. Phải ghi nhận thêm thu ngân hoàn tiền
                        case OrderStatusEnum.Destroy:
                            // TH currentt status là đang vận chuyển or giao hàng không thành công mới ghi nhận cashierRefund
                            if (currentStatus != OrderStatusEnum.Delivering && currentStatus != OrderStatusEnum.Failure)
                                cashierRefund = string.Empty;

                            // TH Hủy đơn do bếp k adapt được khách => cần update thêm request cs báo lại khách

                            if ((currentStatus == OrderStatusEnum.PushToPOS
                                || currentStatus == OrderStatusEnum.KitchenAccept
                                || currentStatus == OrderStatusEnum.KitchenDone
                                || currentStatus == OrderStatusEnum.Delivering
                                || currentStatus == OrderStatusEnum.Failure) && order.request_type != RequestTypeEnum.CSRequestDestroy.GetHashCode())
                            {
                                changeRequestType = (short)RequestTypeEnum.KitchenDestroy;

                                // Push notify
                                _notifyService.PushNotifyToAccounts(orderId.ToString(), RequestTypeEnum.KitchenDestroy);
                            }

                            // TH Hủy đơn CS báo hủy => request type đổi sang Hủy đơn theo yêu cầu của CSKH và không cần note lý do nữa
                            if (changeStatus == OrderStatusEnum.Destroy && order.request_type == RequestTypeEnum.CSRequestDestroy.GetHashCode())
                            {
                                changeRequestType = (short)RequestTypeEnum.ConfirmDestroyForCS;
                                reasonNote = order.reason_note;
                            }


                            result = _orderDal.UpdateStatus(orderId, changeStatusShort, changeRequestType, user, reasonNote: reasonNote, cashierReceive: order.cashier_receive, cashierRefund: cashierRefund);

                            break;
                        #endregion
                        default:
                            result = _orderDal.UpdateStatus(orderId, changeStatusShort, changeRequestType, user, reasonNote: order.reason_note, cashierReceive: order.cashier_receive, cashierRefund: order.cashier_refund);
                            break;
                    }

                    if (result <= 0) errorCodes = ErrorCodes.BusinessError;
                    else
                    {
                        // history khi thay đổi trạng thái sẽ full các trường
                        _orderHistoryBo.Insert(new OrderHistory
                        {
                            change_log = reasonNote,
                            created_by = user,
                            order_id = orderId,
                            status = changeStatusShort,
                            request_type = changeRequestType,
                            delivery_status = order.delivery_status
                        });

                        errorCodes = ErrorCodes.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                errorCodes = ErrorCodes.Exception;
            }
            return errorCodes;
        }

        /// <summary>
        /// The update delivery status
        /// Author: ThanhDT
        /// Created date: 8/11/2020 2:56 PM
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="deliveryStatus">The delivery status.</param>
        /// <param name="cancelStatus">The cancel status.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public ErrorCodes UpdateDeliveryStatus(int orderId, short deliveryStatus, short cancelStatus, string user = "")
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                var result = _orderDal.UpdateDeliveryStatus(orderId, deliveryStatus, cancelStatus, user);


                if (result <= 0) errorCodes = ErrorCodes.BusinessError;
                else
                {
                    // history khi thay đổi deliveryStatus sẽ không có thông tin status và request type
                    _orderHistoryBo.Insert(new OrderHistory
                    {
                        change_log = "",
                        created_by = user,
                        order_id = orderId,
                        delivery_status = deliveryStatus
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                errorCodes = ErrorCodes.Exception;
            }
            return errorCodes;
        }
        public ErrorCodes UpdateRequestType(int orderId, RequestTypeEnum type, string reasonNote = "", string user = "")
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                var result = _orderDal.UpdateRequestType(orderId, (short)type, reasonNote, user);


                if (result <= 0) errorCodes = ErrorCodes.BusinessError;
                else
                {
                    // history khi thay đổi request type sẽ không có thông tin status và delivery status
                    _orderHistoryBo.Insert(new OrderHistory
                    {
                        change_log = reasonNote,
                        created_by = user,
                        order_id = orderId,
                        request_type = (short)type
                    });

                    // Push notify
                    if (type == RequestTypeEnum.CSRequestDestroy || type == RequestTypeEnum.KitchenDestroy)
                        _notifyService.PushNotifyToAccounts(orderId.ToString(), type);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                errorCodes = ErrorCodes.Exception;
            }
            return errorCodes;
        }

        public IEnumerable<OrderOnListModel> GetList(string keyword, short status, short sourceType, short deliveryStatus, short orderType, DateTime? fromCreatedDate, DateTime? toCreatedDate, short requestType, out int totalRows, int? pageIndex = 1, int? pageSize = 15)
        {
            try
            {
                var result = new List<OrderOnListModel>();
                var lst = _orderDal.GetList(keyword, status, sourceType, deliveryStatus, orderType, fromCreatedDate, toCreatedDate, requestType, out totalRows, pageIndex, pageSize);
                if (lst != null && lst.Any())
                    result = lst.Select(x => new OrderOnListModel(x)).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                totalRows = 0;
                return new List<OrderOnListModel>();
            }
        }

        public OrderViewModel GetById(int id)
        {
            OrderViewModel orderModel = new OrderViewModel();
            try
            {
                var order = _orderDal.GetById(id);
                if (order != null && order.order_id > 0)
                {
                    var lstDistrict = _districtBoCached.GetAll().ToList();
                    var lstWard = _wardBoCached.GetAll().ToList();
                    var district = lstDistrict.Find(x => x.district_id == order.district_id);
                    var ward = lstWard.Find(x => x.ward_id == order.ward_id);
                    var listOrderDetail = _orderDetailDal.GetByOrderId(id).Select(x => new OrderDetailViewModel(x)).ToList();

                    orderModel = new OrderViewModel(order, district, ward, listOrderDetail);
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return orderModel;
        }

        public OrderFullViewModel GetByFullValueById(int id)
        {
            OrderFullViewModel orderModel = new OrderFullViewModel();
            try
            {
                var order = _orderDal.GetById(id);
                if (order != null && order.order_id > 0)
                {
                    var lstDistrict = _districtBoCached.GetAll().ToList();
                    var lstWard = _wardBoCached.GetAll().ToList();
                    var district = lstDistrict.Find(x => x.district_id == order.district_id);
                    var ward = lstWard.Find(x => x.ward_id == order.ward_id);
                    var listOrderDetail = _orderDetailDal.GetByOrderId(id).Select(x => new OrderDetailViewModel()).ToList();

                    orderModel = new OrderFullViewModel(order, district, ward, listOrderDetail);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return orderModel;
        }

        public IEnumerable<OrderDetailViewModel> GetByOrderId(int id)
        {
            try
            {
                //return _orderDetailDal.GetAllByOrderId(id);
                return _orderDetailDal.GetByOrderId(id).Select(x => new OrderDetailViewModel(x));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return new List<OrderDetailViewModel>();
        }

        /// <summary>
        /// The get by delivery order identifier
        /// Author: ThanhDT
        /// Created date: 8/11/2020 12:07 AM
        /// </summary>
        /// <param name="deliveryOrderId">The delivery order identifier.</param>
        /// <returns></returns>
        public Entities.Order GetByDeliveryOrderId(string deliveryOrderId)
        {
            try
            {
                //return _orderDetailDal.GetAllByOrderId(id);
                return _orderDal.GetByDeliveryOrderId(deliveryOrderId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }

        /// <summary>
        /// The validate change status
        /// Author: ThanhDT
        /// Created date: 8/9/2020 2:41 PM
        /// </summary>
        /// <param name="currentStatus">The current status.</param>
        /// <param name="changeStatus">The change status.</param>
        /// <returns></returns>
        public static bool ValidateChangeStatus(OrderStatusEnum currentStatus, OrderStatusEnum changeStatus)
        {
            switch (currentStatus)
            {
                case OrderStatusEnum.Inactive:
                    return changeStatus == OrderStatusEnum.Pending
                        || changeStatus == OrderStatusEnum.Destroy;
                case OrderStatusEnum.Pending:
                    return changeStatus == OrderStatusEnum.PushToPOS
                        || changeStatus == OrderStatusEnum.Destroy;
                case OrderStatusEnum.PushToPOS:
                    return changeStatus == OrderStatusEnum.KitchenAccept
                        || changeStatus == OrderStatusEnum.Destroy;
                case OrderStatusEnum.KitchenAccept:
                    return changeStatus == OrderStatusEnum.KitchenDone
                        || changeStatus == OrderStatusEnum.Destroy;
                case OrderStatusEnum.KitchenDone:
                    return changeStatus == OrderStatusEnum.Delivering
                        || changeStatus == OrderStatusEnum.Success
                        || changeStatus == OrderStatusEnum.Failure
                        || changeStatus == OrderStatusEnum.Destroy;
                case OrderStatusEnum.Delivering:
                    return changeStatus == OrderStatusEnum.Success
                        || changeStatus == OrderStatusEnum.Failure
                        || changeStatus == OrderStatusEnum.Destroy;
                case OrderStatusEnum.Failure:
                    return changeStatus == OrderStatusEnum.Success
                        || changeStatus == OrderStatusEnum.Destroy;
                case OrderStatusEnum.Success:
                default:
                    return changeStatus == OrderStatusEnum.Destroy;
            }
        }

        /// <summary>
        /// The update complete status
        /// Author: ThanhDT
        /// Created date: 8/11/2020 2:53 PM
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="changeStatus">The change status.</param>
        /// <param name="deliveryStatus">The delivery status.</param>
        /// <param name="reasonNote">The reason note.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public ErrorCodes UpdateCompleteStatus(int orderId, OrderStatusEnum changeStatus, DeliveryStatus deliveryStatus, string reasonNote = "", string user = "")
        {
            Entities.Order order;
            try
            {
                order = _orderDal.GetById(orderId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ErrorCodes.Exception;
            }
            if (order == null || order.order_id == 0)
            {
                return ErrorCodes.UnknowError;
            }

            //var currentStatus = (OrderStatusEnum)order.status;
            // Bắt buộc trạng thái trước đó phải là Delivering
            /*if (currentStatus != OrderStatusEnum.Delivering || !ValidateChangeStatus(currentStatus, changeStatus))
            {
                return ErrorCodes.StatusError;
            }*/

            var changeStatusShort = (short)changeStatus;
            int result;
            try
            {
                result = _orderDal.UpdateCompleteStatus(orderId, changeStatusShort, (short)deliveryStatus, user, reasonNote: order.reason_note);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ErrorCodes.Exception;
            }

            ErrorCodes errorCodes;
            if (result <= 0) errorCodes = ErrorCodes.BusinessError;
            else
            {
                _orderHistoryBo.Insert(new OrderHistory
                {
                    change_log = reasonNote,
                    created_by = user,
                    order_id = orderId,
                    status = changeStatusShort
                });

                errorCodes = ErrorCodes.Success;
            }

            return errorCodes;
        }

        public IEnumerable<CashierRevenueStatisticModel> GetCashierRevenueStatistic(DateTime fromCreatedDate, DateTime toCreatedDate, int Type, out List<DateTime> LstDate)
        {

            try
            {
                var lstOrder = _orderDal.GetList((short)OrderStatusEnum.Success, fromCreatedDate, toCreatedDate);
                if (lstOrder.Any())
                {
                    LstDate = EachDay(fromCreatedDate, toCreatedDate).ToList();
                    var lstCashier = lstOrder.Select(x => x.cashier_receive).GroupBy(x => x).OrderBy(x => x.Key).ToList();
                    var result = new List<CashierRevenueStatisticModel>();
                    foreach (var cashier in lstCashier)
                    {
                        var total = 0;
                        var cashierRevenueObj = new CashierRevenueStatisticModel { CashierName = cashier.Key };
                        foreach (DateTime day in EachDay(fromCreatedDate, toCreatedDate))
                        {
                            var revenueByDay = new CashierRevenueStatisticByDayModel { Day = day };
                            var RevenueOfDishes = lstOrder.Where(x => x.cashier_receive == cashier.Key
                            && x.created_date.Date.CompareTo(day) == 0).Sum(x => x.price);
                            var ShippingFee = lstOrder.Where(x => x.cashier_receive == cashier.Key
                            && x.created_date.Date.CompareTo(day) == 0).Sum(x => x.ship_fee);
                            if (Type == 0)
                                revenueByDay.Revenue = RevenueOfDishes;
                            else if (Type == 1)
                                revenueByDay.Revenue = ShippingFee;
                            else
                                revenueByDay.Revenue = ShippingFee + RevenueOfDishes;
                            cashierRevenueObj.lstDay.Add(revenueByDay);
                            total += revenueByDay.Revenue;
                        }
                        cashierRevenueObj.TotalStr = StringUtils.ConvertNumberToCurrency(total);
                        result.Add(cashierRevenueObj);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            LstDate = new List<DateTime>();
            return new List<CashierRevenueStatisticModel>();
        }

        public IEnumerable<CashierRevenueStatisticModel> GetCashierRevenueStatisticBySourceType(DateTime fromCreatedDate, DateTime toCreatedDate, int Type)
        {
            try
            {
                var lstOrder = _orderDal.GetList((short)OrderStatusEnum.Success, fromCreatedDate, toCreatedDate);
                if (lstOrder.Any())
                {
                    var lstSourceType = EnumHelper.Instance.ConvertEnumToList<SourceTypeEnum>().ToList();
                    var result = new List<CashierRevenueStatisticModel>();
                    foreach (var item in lstSourceType)
                    {
                        var total = 0;
                        var cashierRevenueObj = new CashierRevenueStatisticModel { CashierName = item.Name };
                        foreach (DateTime day in EachDay(fromCreatedDate, toCreatedDate))
                        {
                            var revenueByDay = new CashierRevenueStatisticByDayModel { Day = day };
                            var RevenueOfDishes = lstOrder.Where(x => x.source_type == item.Id
                            && x.created_date.Date.CompareTo(day) == 0).Sum(x => x.price);
                            var ShippingFee = lstOrder.Where(x => x.source_type == item.Id
                            && x.created_date.Date.CompareTo(day) == 0).Sum(x => x.ship_fee);
                            if (Type == 0)
                                revenueByDay.Revenue = RevenueOfDishes;
                            else if (Type == 1)
                                revenueByDay.Revenue = ShippingFee;
                            else
                                revenueByDay.Revenue = ShippingFee + RevenueOfDishes;
                            cashierRevenueObj.lstDay.Add(revenueByDay);
                            total += revenueByDay.Revenue;
                        }
                        cashierRevenueObj.TotalStr = StringUtils.ConvertNumberToCurrency(total);
                        result.Add(cashierRevenueObj);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return new List<CashierRevenueStatisticModel>();
        }
        public IEnumerable<CashierRevenueStatisticModel> GetCashierRevenueStatisticByPaymentType(DateTime fromCreatedDate, DateTime toCreatedDate, int Type)
        {
            try
            {
                var lstOrder = _orderDal.GetList((short)OrderStatusEnum.Success, fromCreatedDate, toCreatedDate);
                if (lstOrder.Any())
                {
                    var lstPaymentType = EnumHelper.Instance.ConvertEnumToList<PaymentTypeEnum>().ToList();
                    var result = new List<CashierRevenueStatisticModel>();
                    foreach (var item in lstPaymentType)
                    {
                        var total = 0;
                        var cashierRevenueObj = new CashierRevenueStatisticModel { CashierName = item.Name };
                        foreach (DateTime day in EachDay(fromCreatedDate, toCreatedDate))
                        {
                            var revenueByDay = new CashierRevenueStatisticByDayModel { Day = day };
                            var RevenueOfDishes = lstOrder.Where(x => x.source_type == item.Id
                            && x.created_date.Date.CompareTo(day) == 0).Sum(x => x.price);
                            var ShippingFee = lstOrder.Where(x => x.source_type == item.Id
                            && x.created_date.Date.CompareTo(day) == 0).Sum(x => x.ship_fee);
                            if (Type == 0)
                                revenueByDay.Revenue = RevenueOfDishes;
                            else if (Type == 1)
                                revenueByDay.Revenue = ShippingFee;
                            else
                                revenueByDay.Revenue = ShippingFee + RevenueOfDishes;
                            cashierRevenueObj.lstDay.Add(revenueByDay);
                            total += revenueByDay.Revenue;
                        }
                        cashierRevenueObj.TotalStr = StringUtils.ConvertNumberToCurrency(total);
                        result.Add(cashierRevenueObj);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return new List<CashierRevenueStatisticModel>();
        }


        public IEnumerable<CashierRevenueStatisticModel> GetCashierRevenueStatisticByProduct(DateTime fromCreatedDate, DateTime toCreatedDate)
        {
            try
            {
                var lstOrderDetail = _orderDetailDal.GetList((short)OrderStatusEnum.Success, fromCreatedDate, toCreatedDate);

                if (lstOrderDetail.Any())
                {
                    var result = new List<CashierRevenueStatisticModel>();
                    var lstProduct = lstOrderDetail.GroupBy(x => x.product_id).Select(c => c.First()).ToList();
                    foreach (var item in lstProduct)
                    {
                        var total = 0;
                        var cashierRevenueObj = new CashierRevenueStatisticModel { CashierName = item.product_name };
                        foreach (DateTime day in EachDay(fromCreatedDate, toCreatedDate))
                        {
                            var revenueByDay = new CashierRevenueStatisticByDayModel { Day = day };
                            var RevenueOfDishes = lstOrderDetail.Where(x => x.product_id == item.product_id
                            && x.created_date.Date.CompareTo(day) == 0).Sum(x => x.price);
                            revenueByDay.Revenue = RevenueOfDishes;
                            cashierRevenueObj.lstDay.Add(revenueByDay);
                            total += revenueByDay.Revenue;
                        }
                        cashierRevenueObj.TotalStr = StringUtils.ConvertNumberToCurrency(total);
                        result.Add(cashierRevenueObj);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return new List<CashierRevenueStatisticModel>();
        }
        public IEnumerable<CashierRevenueStatisticModel> CountCashierRevenueStatisticByProduct(DateTime fromCreatedDate, DateTime toCreatedDate)
        {
            try
            {
                var lstOrderDetail = _orderDetailDal.GetList((short)OrderStatusEnum.Success, fromCreatedDate, toCreatedDate);

                if (lstOrderDetail.Any())
                {
                    var result = new List<CashierRevenueStatisticModel>();
                    var lstProduct = lstOrderDetail.GroupBy(x => x.product_id).Select(c => c.First()).ToList();
                    foreach (var item in lstProduct)
                    {
                        var total = 0;
                        var cashierRevenueObj = new CashierRevenueStatisticModel { CashierName = item.product_name };
                        foreach (DateTime day in EachDay(fromCreatedDate, toCreatedDate))
                        {
                            var revenueByDay = new CashierRevenueStatisticByDayModel { Day = day };
                            var RevenueOfDishes = lstOrderDetail.Where(x => x.product_id == item.product_id
                            && x.created_date.Date.CompareTo(day) == 0).Count();
                            revenueByDay.Revenue = RevenueOfDishes;
                            cashierRevenueObj.lstDay.Add(revenueByDay);
                            total += revenueByDay.Revenue;
                        }
                        cashierRevenueObj.TotalStr = StringUtils.ConvertNumberToCurrency(total);
                        result.Add(cashierRevenueObj);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return new List<CashierRevenueStatisticModel>();
        }

        public IEnumerable<CashierRevenueStatisticModel> CountOrderAndCustomer(DateTime fromCreatedDate, DateTime toCreatedDate)
        {
            try
            {
                var lstOrder = _orderDal.GetList((short)OrderStatusEnum.Success, fromCreatedDate, toCreatedDate);
                if (lstOrder.Any())
                {
                    var result = new List<CashierRevenueStatisticModel>();

                    var total = 0;
                    var cusTotal = 0;
                    var countBillRevenueObj = new CashierRevenueStatisticModel { CashierName = "Số lượng bill" };
                    var countCusRevenueObj = new CashierRevenueStatisticModel { CashierName = "Số lượng khách hàng" };
                    foreach (DateTime day in EachDay(fromCreatedDate, toCreatedDate))
                    {
                        var countBillByDay = new CashierRevenueStatisticByDayModel { Day = day };
                        var countCusByDay = new CashierRevenueStatisticByDayModel { Day = day };
                        //tính số lượng bill
                        var CountBill = lstOrder.Where(x => x.created_date.Date.CompareTo(day) == 0).Count();
                        //tính số lượng khách hàng ko có CustomerId
                        var CountCusNoId = lstOrder.Where(x => x.customer_id == 0 && x.created_date.Date.CompareTo(day) == 0).Count();
                        var CountCus = lstOrder.Where(x => x.customer_id != 0 && x.created_date.Date.CompareTo(day) == 0).GroupBy(x => x.customer_id).Select(c => c.First()).Count();
                        countBillByDay.Revenue = CountBill;
                        countCusByDay.Revenue = CountCusNoId + CountCus;
                        countBillRevenueObj.lstDay.Add(countBillByDay);
                        countCusRevenueObj.lstDay.Add(countCusByDay);
                        total += countBillByDay.Revenue;
                        cusTotal += countCusByDay.Revenue;
                    }
                    countBillRevenueObj.TotalStr = StringUtils.ConvertNumberToCurrency(total);
                    countCusRevenueObj.TotalStr = StringUtils.ConvertNumberToCurrency(cusTotal);
                    result.Add(countBillRevenueObj);
                    result.Add(countCusRevenueObj);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return new List<CashierRevenueStatisticModel>();
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        public IEnumerable<OrderCountModel> CountOrder()
        {
            return _orderDal.CountOrder().Select(x => new OrderCountModel(x));
        }

        public ErrorCodes ProcessCallAhamove(int orderId, string user)
        {
            try
            {
                var order = _orderDal.GetById(orderId);
                if (order != null && order.order_id > 0)
                {
                    // validate
                    if (order.status != OrderStatusEnum.KitchenDone.GetHashCode() || order.order_type != OrderTypeEnum.Delivery.GetHashCode())
                        return ErrorCodes.StatusError;

                    if (order.delivery_status > 0 && order.delivery_status != DeliveryStatus.CallAnotherShipping.GetHashCode())
                        return ErrorCodes.AhamoveExistError;

                    PathInfo kitchenPath = new PathInfo { Address = StaticVariable.Kitchen1Address };
                    PathInfo customerPath = new PathInfo { Address = order.delivery_address, Name = order.customer_name, Mobile = order.customer_phone };
                    var orderDetails = _orderDetailDal.GetByOrderId(order.order_id);
                    var orderResponse = AhaMoveApi.CreateOrder(kitchenPath, customerPath, orderDetails.ToList());
                    if (orderResponse != null)
                    {
                        var orderInfo = new Entities.Order
                        {
                            order_id = order.order_id,
                            //status = (short)OrderStatusEnum.Delivering,
                            delivery_status = (short)DeliveryStatus.Idle,
                            delivery_order_id = orderResponse.order_id,
                            delivery_price = orderResponse.order.total_pay,
                            delivery_estimate_time = orderResponse.order.duration,
                            reason_note = "ChangeToDelivery"
                        };
                        var result = _orderDal.ChangeToDelivery(orderInfo);
                        if (result != 1)
                        {
                            Logger.ErrorLog("[ChangeToDelivery] Error save Delivery with OrderId: " + order.order_id);
                            return ErrorCodes.BusinessError;
                        }
                        else
                        {
                            // ok thì bắn vào order history
                            _orderHistoryBo.Insert(new OrderHistory
                            {
                                change_log = "ChangeToDelivery",
                                created_by = user,
                                order_id = orderId,
                                delivery_status = (short)DeliveryStatus.Idle
                            });

                            Logger.WriteLog(Logger.LogType.Info, "[ChangeToDelivery] SUCCESS save Delivery with OrderId: " + order.order_id);
                            return ErrorCodes.Success;
                        }
                    }
                    else
                    {
                        Logger.ErrorLog("[ChangeToDelivery] Error change DeliveryStatus to ApiError with orderId: " + order.order_id);
                        return ErrorCodes.AhamoveError;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ErrorCodes.Exception;
            }

            return ErrorCodes.BusinessError;
        }
        public ErrorCodes UpdateDeliveryPrice(int orderId, string user, int deliveryPrice)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                var deliveryStatus = (short)DeliveryStatus.CallAnotherShipping;
                var result = _orderDal.UpdateDeliveryPrice(orderId, user, deliveryPrice, deliveryStatus);


                if (result <= 0) errorCodes = ErrorCodes.BusinessError;
                else
                {
                    _orderHistoryBo.Insert(new OrderHistory
                    {
                        change_log = "CallAnotherShipping",
                        created_by = user,
                        order_id = orderId,
                        delivery_status = deliveryStatus
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                errorCodes = ErrorCodes.Exception;
            }
            return errorCodes;
        }

        public IEnumerable<SuggestAddressModel> GetSuggestAddress (string keyword)
        {
            var lst = new List<SuggestAddressModel>();
            try
            {
                var model = new SearchSuggestAddressModel();
                model.text = keyword;

                var apiurl = string.Format(StaticVariable.APISuggestAddress, model.text, model.rows);
                string strRes = Utils.MakeGetRequest(apiurl);
                if (!string.IsNullOrWhiteSpace(strRes))
                {
                    var lstResAPI = NewtonJson.Deserialize<ResponseAddressModel>(strRes);
                    lst = lstResAPI.suggestion.Select(x => new SuggestAddressModel(x)).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return lst;
        }

        public IEnumerable<CustomerAddressModel> SearchCustomerByPhone(string keyword)
        {
            return _orderDal.SearchCustomerByPhone(keyword).Select(x => new CustomerAddressModel(x));
        }
    }
}
