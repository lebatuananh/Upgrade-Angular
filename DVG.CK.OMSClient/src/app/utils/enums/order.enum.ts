export class OrderStatusEnum {
  public static readonly Approved = 5;
  // [Description("Chờ xác nhận")]
  public static readonly Inactive = 0;
  // [Description("Chờ đẩy sang bếp")]
  public static readonly Pending = 1;
  // [Description("Đã đẩy sang bếp")]
  public static readonly PushToPOS = 2;
  // [Description("Bếp nhận nấu")]
  public static readonly KitchenAccept = 3;
  // [Description("Bếp báo xong")]
  public static readonly KitchenDone = 4;
  // [Description("Đang vận chuyển")]
  public static readonly Delivering = 5;
  // [Description("Giao hàng không thành công")]
  public static readonly Failure = 6;
  // [Description("Giao hàng thành công")]
  public static readonly Success = 7;
  // [Description("Hủy đơn hàng")]
  public static readonly Destroy = 8;
}

export class RequestTypeEnum {

  // [Description("Không có yêu cầu")]
  public static readonly Normal = 0;
  // [Description("CSKH báo hủy đơn")]
  public static readonly CSRequestDestroy = 1;
  // [Description("Hủy đơn theo yêu cầu của CSKH")]
  public static readonly ConfirmDestroyForCS = 2;
  // [Description("Báo lại khách do bếp hủy")]
  public static readonly KitchenDestroy = 3;
  // [Description("Hủy đơn đã báo lại khách")]
  public static readonly ConfirmCustomerForDestroy = 4;
}

export class DeliveryStatusEnum {
  // [Description("Chưa giao hàng")]
  public static readonly Default = 0;
  // [Description("IDLE")]
  public static readonly Idle = 1;
  // [Description("ASSIGNING")]
  public static readonly Asigning = 2;
  // [Description("ACCEPTED")]
  public static readonly Accepted = 3;
  // [Description("IN PROCESS")]
  public static readonly InProcess = 4;
  // [Description("COMPLETED")]
  public static readonly Completed = 5;
  // [Description("CANCELLED")]
  public static readonly Cancelled = 6;
  // [Description("API ERROR")]
  public static readonly ApiError = 7;
  // [Description("Đơn gọi ship ngoài")]
  public static readonly CallAnotherShipping = 100;
}

export class OrderTypeEnum {
  // [Description("Giao hàng tận nơi")]
  public static readonly Delivery = 0;
  // [Description("Lấy trực tiếp")]
  public static readonly TakeAway = 1;
}
