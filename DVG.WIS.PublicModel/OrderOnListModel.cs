using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.PublicModel
{
    public class OrderOnListModel
    {
        public OrderOnListModel(Entities.Order order)
        {
            OrderId = order.order_id;
            EncryptId = EncryptUtility.EncryptId(order.order_id.ToString());
            Code = order.code;
            OrderCode = order.order_code;
            CustomerName = order.customer_name;
            DistrictId = order.district_id;
            DeliveryAddress = order.delivery_address;
            CustomerPhone = order.customer_phone;
            CustomerNote = order.customer_note;
            CreatedBy = order.created_by;
            ModifiedBy = order.modified_by;
            ReasonNote = order.reason_note;
            Status = order.status;
            StatusStr = Utils.GetEnumDescription((OrderStatusEnum)order.status);
            DeliveryStatus = order.delivery_status;
            RequestType = order.request_type;
            DeliveryStatusStr = Utils.GetEnumDescription((DeliveryStatus)order.delivery_status);
            RequestTypeStr = Utils.GetEnumDescription((RequestTypeEnum)order.request_type);
            CreatedDateStr = order.created_date.ToString("dd/MM/yyyy HH:mm:ss");
            ModifiedDateStr = order.modified_date.ToString("dd/MM/yyyy HH:mm:ss");
            if (order.delivery_date != DateTime.MinValue)
                DeliveryDateStr = order.delivery_date.Value.ToString("dd/MM/yyyy HH:mm");
            else
                DeliveryDateStr = "Giao ngay";
            PriceStr = StringUtils.ConvertNumberToCurrency(order.price);
            OriginPriceStr = StringUtils.ConvertNumberToCurrency(order.origin_price);
            ShipFeeStr = StringUtils.ConvertNumberToCurrency(order.ship_fee);
            SourceTypeStr = Utils.GetEnumDescription((SourceTypeEnum)order.source_type);
            OrderTypeStr = Utils.GetEnumDescription((OrderTypeEnum)order.order_type);
            OrderType = order.order_type;
        }
        public string EncryptId { get; set; }
        public int OrderId { get; set; }
        public Guid Code { get; set; }
        public string OrderCode { get; set; }
        public string CustomerName { get; set; }
        public int DistrictId { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryAddressFull { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerNote { get; set; }
        public string CreatedDateStr { get; set; }
        public string ModifiedDateStr { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ReasonNote { get; set; }
        public short Status { get; set; }
        public string StatusStr { get; set; }
        public string DistrictStr { get; set; }
        public string PriceStr { get; set; }
        public string OriginPriceStr { get; set; }
        public string ShipFeeStr { get; set; }
        public string SourceTypeStr { get; set; }
        public string OrderTypeStr { get; set; }
        public short OrderType { get; set; }
        public short DeliveryStatus { get; set; }
        public string DeliveryStatusStr { get; set; }
        public short RequestType { get; set; }
        public string RequestTypeStr { get; set; }
        public String DeliveryDateStr { get; set; }
        public IEnumerable<OrderDetailViewModel> lstOrderDetail { get; set; }
    }


    public class OrderCountModel
    {
        public Int64 Count { get; set; }
        public int Status { get; set; }
        public int RequestType { get; set; }

        public OrderCountModel(OrderCount orderCount)
        {
            this.Count = orderCount.count;
            this.Status = orderCount.status;
            this.RequestType = orderCount.requesttype;
        }
    }
}
