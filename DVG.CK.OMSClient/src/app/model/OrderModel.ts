export class OrderSearchModel {
  public KeyWord: string;
  public RequestType: number;
  public DeliveryStatus: number;
  public OrderType: number;
  public SourceType: number;
  public Status: number;
  public FromCreatedDate: Date;
  public ToCreatedDate: Date;
  public PageIndex: number;
  public PageSize: number;
  public ArrCreatedDate: any[] = [];
}

export class OrderIndexModel {
  public ListDeliveryStatus: any[] = [];
  public ListSourceType: any[] = [];
  public ListOrderType: any[] = [];
  public Status: number;
  public RequestType: number;
  public Role: number;
  public PrintKitchenApi: string;
  public PrintCashierApi: string;
  public ListOrderCountModel: OrderCountModel[] = [];
}


export class OrderFullViewModel {
  public ListDeliveryStatus: any[] = [];
  public ListSourceType: any[] = [];
  public ListOrderType: any[] = [];
  public ListProductViewModel: OrderDetailViewModel[];
  public ProductViewModelItem: OrderDetailViewModel;
  public OrderOriginPriceStr: string;
  public DiscountStr: string;
  public OrderPriceStr: string;
  public OrderPrice: number;
  public OrderOriginPrice: number;
  public Discount: number;
  public DistrictId: number;
  public WardId: number;
  public SourceType: number;
  public OrderType: number;
  public CustomerName: string;
  public CustomerNote: string;
  public CustomerPhone: string;
  public DeliverDate: string;
  public DeliverDateStr: string;
  public DeliveryAddress: string;
  public ListDistrict: any;
  public ListWard: any;
  public ShipFee: number;
  public ShipFeeStr: string;
  public Status: number;
  public OrderId: number;
  public Code: number;
  public RequestType: number;
  public DeliveryStatus: number;
  public DeliveryDateStr: string;
  public CreatedDateStr: string;
  public CreatedBy: string;
  public ModifiedBy: string;
  public CashierReceive: string;
  public CashierRefund: string;
  public StatusStr: string;
  public DeliveryStatusStr: string;
  public ModifiedDateStr: string;
  public OrderCode: number;
  public TabIndex: number;

  public lstOrderDetailViewModel: OrderDetailViewModel[];
}

export class OrderDetailViewModel {
  public OrderDetailId: number;
  public OrderId: number;
  public ProductId: number;
  public ProductName: string;
  public Quantity: number;
  public OriginPrice: number;
  public Price: number;
  public Note: string;
  public OriginPriceStr: string;
  public PriceStr: string;
  public TotalMoney: number;
  public TotalMoneyStr: string;
  public Code: number;
  public Status: number;
}

export class OrderCountModel {
  public Count: number;
  public Status: number;
  public RequestType: number;
}
