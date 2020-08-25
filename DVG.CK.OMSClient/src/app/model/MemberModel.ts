
export class MemberInfo {
  public Id: number;
  public Displayname: string;
  public Address: string;
  public Mobilealias: string;
  public Email: string;
  public Trusted: boolean;
  public Verify: boolean;
}

export class MemberGetListConditions {
  public WebId = 1;
  public PageIndex: number;
  public PageSize: number;
  public Keyword: string;
  public Islock: boolean;
  public Id: number;
  public StartDate: Date;
  public EndDate: Date;
  public MobilePrefix: string;
  public MobilePrefixName: string;
  public ListMobilePrefix: string[];
  public ListTypeSelected: number[];
  public ProductCountConditions: string;
  public ListShowColumnExcel: string[];
  public ListProductCountConditions: string[];
  public RemoveIncorrectPhone: boolean;
  public RemoveDuplicatePhone: boolean;
}
