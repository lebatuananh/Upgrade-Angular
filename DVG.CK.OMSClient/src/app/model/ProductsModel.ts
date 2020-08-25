export class ProductsModel {

}
export class Product_SearchModel {
  public WebId: number;
  public BrandId: number;
  public ModelId: number;
  public SubModelId: number;
  public Year: number;
  public Expired: number;
  public SeconHand: number;
  public FromDate: Date;
  public ToDate: Date;
  public Status: number;
  public KeyWord: string;
  public ProductId: number;
  public PageSize: number;
  public PageIndex = 1;
  public UserType: number;
  public DomainId: number;
  public LastUpdatedUser: number;
  public CheckDuplicate: boolean;
  public ListDuplicated: Array<number>;
  public CreatedUser: number;
  public RegionId: number;
  public CityId: number;
}

export class Product_ChangeStatusModel {
  public WebId: number;
  public ListProductId: number[] = [];
  public ListReasonId: number[] = [];
  public Action: string;
  public Reason: string;
  public LastUpdatedUser: number;
  public Status: number;
  public HistoryType: number;
  public IsPublish: boolean;
  public ListWebId: number[] = [];
  public ProductToWebType: number;
  public OldStatus: number;
}

export class Product_IndexModel {
  public ListBrand: any[] = [];
  public ListModel: any[] = [];
  public ListSubModel: any[] = [];
  public ListWeb: any[] = [];
  public ListTechGroup: any[] = [];
  public ListTech: any[] = [];
  public ListUser: any[] = [];
  public ListDomain: any[] = [];
  public ListUserType: any[] = [];
  public ListAutoStatus: any[] = [];
  public ListCity: any[] = [];
  public ListRegion: any[] = [];
  public ListSecondHand: any[] = [];
  public ListExpried: any[] = [];
  public ListReason: any[] = [];
  public CreatedUser: number;
}

export class Product_GetLinkModel {
  public WebId: number;
  public RepoProductId: number;
  public WebProductId: number;
  public WebUrl: string;
  public ViewCount: number;
  public ImpressionCount: number;
  public FeedbackCount: number;
}

export class Product_EditModel {
  public Productid: number;
  public Title: string;
  public Model: number;
  public Year: number;
  public Numofkm: number;
  public Image: string;
  public AvatarCrop: string;
  public AvatarNoCrop: string;
  public AvatarSizeCrop: string;
  public ImageSizeCrop: string;
  public Price: number;
  public Transmissionid: number;
  public DisplayTime: Date[] = [];
  public StartDateStr: string;
  public EndDateStr: string;
  public Status: number;
  public Viptype: number;
  public Ispublish: boolean;
  public City: number;
  public Maker: number;
  public Version: number;
  public Type: number;
  public Madein: number;
  public Secondhand: number;
  public Color: number;
  public Region: number;
  public Usertype: number;
  public HavePrice: boolean;
  public ListTech: number[] = [];
  public ListImage: any[] = [];
  public ExtendInfo: Product_ExtendInfo;
  public UserInfo: any;
  public WebId: number;
  public ListBrand: any[] = [];
  public ListModel: any[] = [];
  public ListModelBind: any[] = [];
  public ListSubModel: any[] = [];
  public ListSubModelBind: any[] = [];
  public ListTechInfo: any[] = [];
  public ListRegion: any[] = [];
  public ListCity: any[] = [];
  public ListCityBind: any[] = [];
  public ListWeb: any[] = [];
  public ListTechGroup: any[] = [];
  public IsShowNotApprovedButton: boolean;
  public IsShowDisApprovedButton: boolean;
  public IsShowDeleteButton: boolean;
  public IsShowSaveAndPublishButton: boolean;
  public IsShowSaveButton: boolean;
  public ListAutoType: any[] = [];
  public ListColor: any[] = [];
  public ListTransmission: any[] = [];
  public ListMakeIn: any[] = [];
  public ListVipType: any[] = [];
  public ListSecondHand: any[] = [];
  public CheckSumToken: string;
  public ListReason: any[] = [];
  public ViewDomain: string;
  public PriceStr: string;
  constructor() {
    this.ExtendInfo = new Product_ExtendInfo();
  }
}
export class Product_ExtendInfo {
  public Desctiption: string;
  public Videourl: string;
  public Specid: string;
}
