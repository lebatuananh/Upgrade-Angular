using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;

namespace DVG.WIS.DAL.Order
{
    public interface IOrderDal
    {
        int CreateOrder(OrderFullViewModel model);
        int UpdateOrder(OrderFullViewModel model);
        int UpdateCompleteStatus(int order_id, short status, short deliveryStatus, string updatedBy, string reasonNote);
        int UpdateDeliveryStatus(int orderId, short deliveryStatus, short cancelStatus, string updatedBy);
        int UpdateStatus(int order_id, short status, short requestType, string updatedBy, string reasonNote, string cashierReceive = "", string cashierRefund = "");
        int UpdateRequestType(int orderId, short requestType, string reasonNote, string updatedBy);
        int MoveToKitchen(Entities.Order order);
        int ChangeToDelivery(Entities.Order order);
        IEnumerable<Entities.Order> GetList(string keyword, short status, short sourceType, short deliveryStatus, short orderType, DateTime? fromCreatedDate, DateTime? toCreatedDate, short requestType, out int totalRows, int? pageIndex = 1, int? pageSize = 15);
        IEnumerable<Entities.Order> GetListPendingToKitchen(short status, int limit);
        IEnumerable<Entities.Order> GetListToDelivery(short status, int limit);
        Entities.Order GetById(int id);
        Entities.Order GetByDeliveryOrderId(string deliveryOrderId);
        IEnumerable<Entities.Order> GetList(short status, DateTime fromCreatedDate, DateTime toCreatedDate);
        IEnumerable<Entities.OrderCount> CountOrder();
        IEnumerable<CustomerAddress> SearchCustomerByPhone(string keyword);
        int UpdateDeliveryPrice(int orderId, string user, int deliveryPrice, short deliveryStatus);
    }
}
