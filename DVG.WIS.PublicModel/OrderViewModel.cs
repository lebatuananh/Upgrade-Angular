using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DVG.WIS.PublicModel
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {

        }

        public OrderViewModel(Entities.Order order, Entities.District district, Entities.Ward ward, List<OrderDetailViewModel> lstOrderDetail)
        {
            OrderId = order.order_id;
            Code = order.code;
            OrderCode = order.order_code;
            CustomerName = order.customer_name;
            CityCode = order.city_code;
            DistrictId = order.district_id;
            if(district != null)
            {
                DistrictName = district.district_name;
            }            
            WardId = order.ward_id;
            if(ward != null)
            {
                WardName = ward.ward_name;
            }                
            DeliveryAddress = order.delivery_address;
            DeliveryAddressFull =  string.Format("{0}, {1}, {2}", order.delivery_address, WardName, DistrictName);
            CustomerPhone = order.customer_name;
            CustomerPhone = order.customer_phone;
            CustomerNote = order.customer_note;
            OrderType = order.order_type;
            OrderTypeName = Utils.GetEnumDescription((OrderTypeEnum)order.order_type);
            SourceType = order.source_type;
            SourceTypeName = Utils.GetEnumDescription((SourceTypeEnum)order.source_type);
            RequestType = order.request_type;
            RequestTypeName = Utils.GetEnumDescription((RequestTypeEnum)order.request_type);
            OrderPrice = order.price;
            OrderOriginPrice = order.origin_price;
            OrderPriceStr = StringUtils.ConvertNumberToCurrency(OrderPrice); 
            OrderOriginPriceStr = StringUtils.ConvertNumberToCurrency(OrderOriginPrice);
            DeliveryStatusStr = Utils.GetEnumDescription((DeliveryStatus)(order.delivery_status));
            CreatedBy = order.created_by;
            ModifiedBy = order.modified_by;
            ReasonNote = order.reason_note;
            Status = order.status;
            StatusStr = Utils.GetEnumDescription((OrderStatusEnum)(order.status));
            ShipFee = order.ship_fee;
            ShipFeeStr = StringUtils.ConvertNumberToCurrency(ShipFee);
            CreatedDateStr = order.created_date.ToString("dd/MM/yyyy HH:mm:ss");
            ModifiedDateStr = order.modified_date.ToString("dd/MM/yyyy HH:mm:ss");
            if(order.delivery_date != DateTime.MinValue)
                DeliveryDateStr = order.delivery_date.Value.ToString("dd/MM/yyyy HH:mm:ss");
            else
                DeliveryDateStr = "Giao ngay";
            CashierReceive = order.cashier_receive;
            CashierRefund = order.cashier_refund;
            DeliveryEstimateTime = order.delivery_estimate_time;
            DeliveryEstimateTimeStr = DeliveryEstimateTime + " phút";
            DeliveryPrice = order.delivery_price;
            DeliveryPriceStr = StringUtils.ConvertNumberToCurrency(DeliveryPrice);
            DeliveryStatus = order.delivery_status;
            if (lstOrderDetail != null && lstOrderDetail.Count > 0)
                this.lstOrderDetailViewModel = lstOrderDetail;
            this.Discount = DVG.WIS.Utilities.AppSettings.Instance.GetString("DiscountValuePercent").ToInt() / 100 * this.OrderOriginPrice;
            this.DiscountStr = StringUtils.ConvertNumberToCurrency(this.Discount);
        }

        public int OrderId { get; set; }
        public Guid Code { get; set; }
        public string OrderCode { get; set; }
        public string CustomerName { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int WardId { get; set; }
        public string WardName { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryAddressFull { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerNote { get; set; }
        public string CreatedDateStr { get; set; }
        public string ModifiedDateStr { get; set; }
        public short OrderType { get; set; }
        public string OrderTypeName { get; set; }
        public short SourceType { get; set; }
        public string SourceTypeName { get; set; }
        public short RequestType { get; set; }
        public string RequestTypeName { get; set; }
        public int OrderPrice { get; set; }
        public int OrderOriginPrice { get; set; }
        public int DeliveryStatus { get; set; }
        public string DeliveryStatusStr { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ReasonNote { get; set; }
        public short Status { get; set; }
        public string StatusStr { get; set; }
        public int ShipFee { get; set; }
        public string ShipFeeStr { get; set; }
        public string CashierReceive { get; set; }
        public string CashierRefund { get; set; }
        public string OrderPriceStr { get; set; }
        public string OrderOriginPriceStr { get; set; }
        public List<OrderDetailViewModel> lstOrderDetailViewModel { get; set; }
        public short DeliveryEstimateTime { get; set; }
        public string DeliveryEstimateTimeStr { get; set; }
        public int DeliveryPrice { get; set; }
        public string DeliveryPriceStr { get; set; }
        public string DeliveryDateStr { get; set; }
        public int Discount { get; set; }
        public string DiscountStr { get; set; }
    }

    public class OrderDetailViewModel
    {
        public OrderDetailViewModel()
        {
            OriginPriceStr = StringUtils.ConvertNumberToCurrency(OriginPrice);
            PriceStr = StringUtils.ConvertNumberToCurrency(Price);
        }

        public OrderDetailViewModel(Product product)
        {
            ProductId = product.product_id;
            ProductName = product.name;
            OriginPrice = product.origin_price;
            Price = product.price;
            Code = product.code;
            OriginPriceStr = StringUtils.ConvertNumberToCurrency(OriginPrice);
            PriceStr = StringUtils.ConvertNumberToCurrency(Price);
        }

        public OrderDetailViewModel(Entities.OrderDetail orderDetail)
        {
            if (orderDetail != null)
            {
                OrderDetailId = orderDetail.order_detail_id;
                ProductId = orderDetail.product_id;
                ProductName = orderDetail.product_name;
                Quantity = orderDetail.quantity;
                OriginPrice = orderDetail.origin_price;
                Price = orderDetail.price;
                Note = orderDetail.note;
                OriginPriceStr = StringUtils.ConvertNumberToCurrency(OriginPrice);
                PriceStr = StringUtils.ConvertNumberToCurrency(Price);
                TotalMoney = Quantity * Price;
                TotalMoneyStr = StringUtils.ConvertNumberToCurrency(TotalMoney);
                Status = orderDetail.status;
                OrderId = orderDetail.order_id;
                Code = orderDetail.code;
            }
        }

        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int OriginPrice { get; set; }
        public int Price { get; set; }
        public string Note { get; set; }
        public string OriginPriceStr { get; set; }
        public string PriceStr { get; set; }
        public int TotalMoney { get; set; }
        public string TotalMoneyStr { get; set; }

        public int Code { get; set; }
        public int Status { get; set; }
    }

    public class OrderFullViewModel : OrderViewModel
    {
        public OrderFullViewModel()
        {
            this.ListSourceType = EnumHelper.Instance.ConvertEnumToList<SourceTypeEnum>().ToList();
            this.ListOrderType = EnumHelper.Instance.ConvertEnumToList<OrderTypeEnum>().ToList();            
        }

        public OrderFullViewModel(Entities.Order order, Entities.District district, Entities.Ward ward, List<OrderDetailViewModel> lstOrderDetail)
        {
            OrderId = order.order_id;
            Code = order.code;
            CustomerName = order.customer_name;
            CityCode = order.city_code;
            DistrictId = order.district_id;
            if (district != null)
                DistrictName = district.district_name;
            WardId = order.ward_id;
            if (ward != null)
                WardName = ward.ward_name;
            DeliveryAddress = order.delivery_address;
            CustomerPhone = order.customer_phone;
            CustomerNote = order.customer_note;
            OrderType = order.order_type;
            OrderTypeName = Utils.GetEnumDescription((OrderTypeEnum)order.order_type);
            SourceType = order.source_type;
            SourceTypeName = Utils.GetEnumDescription((SourceTypeEnum)order.source_type);
            RequestType = order.request_type;
            RequestTypeName = Utils.GetEnumDescription((RequestTypeEnum)order.request_type);
            OrderPrice = order.price;
            OrderOriginPrice = order.origin_price;
            OrderPriceStr = StringUtils.ConvertNumberToCurrency(OrderPrice);
            OrderOriginPriceStr = StringUtils.ConvertNumberToCurrency(OrderOriginPrice);
            DeliveryStatusStr = Utils.GetEnumDescription((DeliveryStatus)(order.delivery_status));
            CreatedBy = order.created_by;
            ModifiedBy = order.modified_by;
            ReasonNote = order.reason_note;
            Status = order.status;
            StatusStr = Utils.GetEnumDescription((OrderStatusEnum)(order.status));
            ShipFee = order.ship_fee;
            ShipFeeStr = StringUtils.ConvertNumberToCurrency(ShipFee);
            CreatedDateStr = order.created_date.ToString("dd/MM/yyyy HH:mm:ss");
            ModifiedDateStr = order.modified_date.ToString("dd/MM/yyyy HH:mm:ss");
            CashierReceive = order.cashier_receive;
            CashierRefund = order.cashier_refund;
            //if (lstOrderDetail != null && lstOrderDetail.Count > 0)
            //    this.lstOrderDetailViewModel = lstOrderDetail;

            if (order.delivery_date != DateTime.MinValue && order.delivery_date != null)
            {
                DeliverDate = order.delivery_date.Value;
                DeliveryDateStr = order.delivery_date.Value.ToString("dd/MM/yyyy HH:mm:ss");
            }
            else
                DeliveryDateStr = "Giao ngay";
            this.ListSourceType = EnumHelper.Instance.ConvertEnumToList<SourceTypeEnum>().ToList();
            this.ListOrderType = EnumHelper.Instance.ConvertEnumToList<OrderTypeEnum>().ToList();
            this.Discount = DVG.WIS.Utilities.AppSettings.Instance.GetString("DiscountValuePercent").ToInt() / 100 * this.OrderOriginPrice;
            this.DiscountStr = StringUtils.ConvertNumberToCurrency(this.Discount);
        }
        public OrderDetailViewModel OrderDetailViewModelItem { get; set; }
        public IEnumerable<DVG.WIS.Utilities.EnumHelper.Enums> ListSourceType { get; set; }
        public IEnumerable<DVG.WIS.Utilities.EnumHelper.Enums> ListOrderType { get; set; }
        public DateTime DeliverDate { get; set; }
        public string DeliverDateStr { get; set; }
        public List<District> ListDistrict { get; set; }
        public List<Ward> ListWard { get; set; }
        public List<OrderDetailViewModel> ListProductViewModel { get; set; }
        public OrderDetailViewModel ProductViewModelItem { get; set; }
        //public int Discount {get;set;}
        //public string DiscountStr {get;set;}
        public int TabIndex { get; set; }
    }
}
