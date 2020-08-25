using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DVG.WIS.Core.Enums
{
    public enum OrderStatusEnum
    {
        //0: Inactive - Chưa kích hoạt; 
        //1: Pending - Chờ đẩy sang POS; 
        //2: PushToPOS - Đã đẩy sang POS; 
        //3: KitchenAccept - Bếp nhận nấu; 
        //4: KitchenDone - Bếp báo done, chuyển Delivery or TakeAway; 
        //5: Delivering - Đang vận chuyển; 
        //6: Failure - Giao hàng không thành công; 
        //7: Success - Giao hàng thành công; 
        //8: Destroy: Hủy đơn hàng;
        [Description("Chờ xác nhận")]
        Inactive = 0,
        [Description("Chờ đẩy sang bếp")]
        Pending = 1,
        [Description("Đã đẩy sang bếp")]
        PushToPOS = 2,
        [Description("Bếp nhận nấu")]
        KitchenAccept = 3,
        [Description("Bếp báo xong")]
        KitchenDone = 4,
        [Description("Đang vận chuyển")]
        Delivering = 5,
        [Description("Giao hàng không thành công")]
        Failure = 6,
        [Description("Giao hàng thành công")]
        Success = 7,
        [Description("Hủy đơn hàng")]
        Destroy = 8
    }

    public enum DeliveryStatus
    {
        // Các trạng thái Delivery: 0: Chưa giao hàng; 1: IDLE; 2:ASSIGNING;3:ACCEPTED;4:IN PROCESS;5:COMPLETED;6:CANCELLED
        [Description("Chưa giao hàng")]
        Default = 0,
        [Description("IDLE")]
        Idle = 1,
        [Description("ASSIGNING")]
        Asigning = 2,
        [Description("ACCEPTED")]
        Accepted = 3,
        [Description("IN PROCESS")]
        InProcess = 4,
        [Description("COMPLETED")]
        Completed = 5,
        [Description("CANCELLED")]
        Cancelled = 6,
        [Description("API ERROR")]
        ApiError = 7,
        [Description("Đơn gọi ship ngoài")]
        CallAnotherShipping = 100
    }

    public enum DeliveryCancelStatus
    {
        // Các trạng thái Delivery: 0: Chưa giao hàng; 1: IDLE; 2:ASSIGNING;3:ACCEPTED;4:IN PROCESS;5:COMPLETED;6:CANCELLED
        [Description("Chưa cancel")]
        Default = 0,
        [Description("Supplier")]
        Supplier = 1,
        [Description("Customer")]
        Customer = 2,
        [Description("Timeout")]
        Timeout = 3
    }

    public enum SourceTypeEnum
    {
        //  0: web, 1: fb, 2: zalo, 3: grab now, 4: call center
        [Description("Từ website")]
        Web = 0,
        [Description("Từ facebook")]
        Facebook = 1,
        [Description("Từ zalo")]
        Zalo = 2,
        [Description("Từ grabnow")]
        GrabNow = 3,
        [Description("Từ call center")]
        CallCenter = 4
    }

    public enum RequestTypeEnum
    {
        [Description("Không có yêu cầu")]
        Normal = 0,
        [Description("CSKH báo hủy đơn")]
        CSRequestDestroy = 1,
        [Description("Hủy đơn theo yêu cầu của CSKH")]
        ConfirmDestroyForCS = 2,
        [Description("Báo lại khách do bếp hủy")]
        KitchenDestroy = 3,
        [Description("Hủy đơn đã báo lại khách")]
        ConfirmCustomerForDestroy = 4
    }

    public enum OrderTypeEnum
    {
        [Description("Giao hàng tận nơi")]
        Delivery = 0,
        [Description("Lấy trực tiếp")]
        TakeAway = 1
    }
    public enum RevenueTypeEnum
    {
        [Description("Doanh thu món ăn")]
        RevenueOfDishes = 0,
        [Description("Phí ship")]
        ShippingFee = 1,
        [Description("Doanh thu và phí ship")]
        RevenueAndShippingFee = 2
    }
    public enum PaymentTypeEnum
    {
        [Description("Tiền mặt")]
        Normal = 0,
        [Description("VNPay")]
        VNPay = 1,
        [Description("ZaloPay")]
        ZaloPay = 2
    }
}
