
export class NotificationModel {
  public Id: number;
  public Title: string;
  public Content: string;
  public BrandId: number;
  public ModelId: number;
  public BrandName: string;
  public ModelName: string;
  public Image: string;
  public DeliveryType: number;
  public DeliveryDate: Date;
  public DestinationLink: string;
}

export class NotificationIndexModel {
  public ListBrand: any[] = [];
  public ListModel: any[] = [];
}

export class NotificationGetListCondition {
  public Keyword: string;
  public BrandId: number;
  public ModelId: number;
  public PageIndex: number;
  public PageSize: number;
}
