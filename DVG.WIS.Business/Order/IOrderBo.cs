using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Business.Order
{
    public interface IOrderBo
    {
        ErrorCodes CreateOrder(OrderFullViewModel model);
        ErrorCodes UpdateStatus(int orderId, OrderStatusEnum changeStatus, string reasonNote = "", string user = "", string cashierRefund = "", bool isCS = false);
        ErrorCodes UpdateDeliveryStatus(int orderId, short deliveryStatus, short cancelStatus, string user = "");
        ErrorCodes UpdateCompleteStatus(int orderId, OrderStatusEnum changeStatus, DeliveryStatus deliveryStatus, string reasonNote = "", string user = "");
        ErrorCodes UpdateRequestType(int orderId, RequestTypeEnum type, string reasonNote = "", string user = "");
        IEnumerable<OrderOnListModel> GetList(string keyword, short status, short sourceType, short deliveryStatus, short orderType, DateTime? fromCreatedDate, DateTime? toCreatedDate, short requestType, out int totalRows, int? pageIndex = 1, int? pageSize = 15);
        OrderViewModel GetById(int id);
        OrderFullViewModel GetByFullValueById(int id);
        IEnumerable<OrderDetailViewModel> GetByOrderId(int id);
        Entities.Order GetByDeliveryOrderId(string deliveryOrderId);
        IEnumerable<CashierRevenueStatisticModel> GetCashierRevenueStatistic(DateTime fromCreatedDate, DateTime toCreatedDate,int Type, out List<DateTime> LstDate);
        IEnumerable<CashierRevenueStatisticModel> GetCashierRevenueStatisticBySourceType(DateTime fromCreatedDate, DateTime toCreatedDate, int Type);
        IEnumerable<CashierRevenueStatisticModel> GetCashierRevenueStatisticByPaymentType(DateTime fromCreatedDate, DateTime toCreatedDate, int Type);
        IEnumerable<CashierRevenueStatisticModel> GetCashierRevenueStatisticByProduct(DateTime fromCreatedDate, DateTime toCreatedDate);
        IEnumerable<CashierRevenueStatisticModel> CountCashierRevenueStatisticByProduct(DateTime fromCreatedDate, DateTime toCreatedDate);
        IEnumerable<CashierRevenueStatisticModel> CountOrderAndCustomer(DateTime fromCreatedDate, DateTime toCreatedDate);
        IEnumerable<OrderCountModel> CountOrder();
        ErrorCodes ProcessCallAhamove(int orderId, string user);
        ErrorCodes UpdateDeliveryPrice(int orderId, string user, int deliveryPrice);
        IEnumerable<SuggestAddressModel> GetSuggestAddress(string keyword);
        IEnumerable<CustomerAddressModel> SearchCustomerByPhone(string keyword);
    }
}
