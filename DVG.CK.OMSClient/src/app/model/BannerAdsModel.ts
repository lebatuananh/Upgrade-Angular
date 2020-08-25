
export class BannerAdsCondition {
  public WebId: number;
  public Keyword: string;
  public PlatformId: number;
  public PageId: number;
  public Position: number;
  public Type: number;
  public Status: number;
  public StartDate: Date;
  public EndDate: Date;
  public PageIndex: number;
  public PageSize: number;
}

export class BannerAdsModel {
  public WebId: number;
  public Id: number;
  public Description: string;
  public Title: string;
  public Alternate: string;
  public Priority: number;
  public PlatformId: number = null;
  public PageId: number = null;
  public Position: number;
  public ImageUrl: string;
  public Image1: string;
  public Image2: string;
  public Image3: string;
  public Image4: string;
  public TargetLink: string;
  public StartDate: Date;
  public EndDate: Date;
  public TimeSlot: number;
  public Type: number;
  public Status: number;
  public EmbededCode: string;
  public ListBannerAdsPage: any[] = [];
  public ListBannerAdsPlatform: any[] = [];
  public ListBannerAdsPosition: any[] = [];
  public ListBannerAdsStatus: any[] = [];
  public ListBannerAdsType: any[] = [];
  public DisplayTime: Date[] = [];
  public StartDateStr: string;
  public EndDateStr: string;
  public PlatformOfBanner = {
    IsMobile: false,

  }
  //public PositionOfBanner = {
  //  IsTopBanner: false,
  //  IsLeftBanner: false,
  //  IsLeftLargeSkyscraperBanner: false,
  //  IsRightLargeSkyscraperBanner: false,
  //  IsRightBanner: false,
  //  IsCenterBanner: false,
  //  IsStickyBanner: false,
  //  IsCenterBanner1: false,
  //  IsBigArticleBanner: false,
  //  IsCenterBanner2: false,
  //};
}
