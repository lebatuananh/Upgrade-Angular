import { DatePipe } from '@angular/common';
export class LeadIndexModel {
  public DicBudget: any[] = [];
  public DicCarCondition: any[] = [];
  public ListRegion: any[] = [];
  public ListCity: any[] = [];
  public ListBrand: any[] = [];
  public ListModel: any[] = [];
  public DicLeadsActionType: any[] = [];
  public DicLeadsStatus: any[] = [];
  public DicLeadsQuality: any[] = [];
  public ListDealer: any[] = [];
}
export class LeadGetListModel {
  public Keyword: string;
  public ListCondition: number[] = [];
  public ListRegion: number[] = [];
  public ListCity: number[] = [];
  public ListBrand: number[] = [];
  public ListModel: number[] = [];
  public ListAction: number[] = [];
  public ListDealerId: number[] = [];
  public Quality: number;
  public Status: number;
  public PageIndex = 1;
  public PageSize = 15;
  public startDateCreate: DatePipe;
  public startDateCreateStr: string;
  public endDateCreate: DatePipe;
  public endDateCreateStr: string;
}
export class LeadAddModel {
  public DicBudget: any[] = [];
  public DicCarCondition: any[] = [];
  public ListRegion: any[] = [];
  public ListCity: any[] = [];
  public ListBrand: any[] = [];
  public ListModel: any[] = [];
  public BuyerInfo: LeadInfoModel;
  public ListDetail: LeadDetailModel[] = [];
}
export class LeadEditModel {
  public BuyerInfo: LeadInfoModel;
  public DicLeadsStatus: any[] = [];
}
export class LeadDetailModel {
  public Id: number;
  public LeadId: number;
  public BrandId: number;
  public ModelId: number;
  public ProductId: number;
  public Year: number;
  public RegionId: number;
  public CityId: number;
  public ActionType: number;
  public CarCondition: number;
  public DealerId: number;
  public Budget: number;
  public Version: number;
  public CarInfoStr: string;
  public ActionTypeStr: string;
  public CreatedDateStr: string;
  public BudgetStr: string;
  public CarConditionStr: string;
  public CityName: string;
  public ListCity: any[] = [];
  public ListModel: any[] = [];
}
export class LeadInfoModel {
  public Id = 0;
  public FullName: string;
  public Email: string;
  public PhoneNumber: string;
  public Status = -1;
  public Remark: string;
  public CreatedDateStr: string;
  public QualityDateStr: string;
}
export class LeadStatisticConditions {
  public FromDate: Date;
  public ToDate: Date;
  public StatisticedData: any;
}
