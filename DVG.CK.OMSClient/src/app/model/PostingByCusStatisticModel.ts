export class PostingByCusStatisticModel {
  public Createdate: number;
  public TotalNews: number;
  public DayGet: Date;
  public DayGetStr: string;
  public WebId: number;
  public MemberType: number;
  public IsPublish: boolean;
}
export class PostingByCusStatisticIndexModel {
  public ListWeb: any[] = [];
  public StartDateIndex: Date;
  public EndDateIndex: Date;
}
export class PostingByCusStatisticGetListModel {
  public ListData: PostingByCusStatisticModel[] = [];
  public ListWeb: any[] = [];
  public WebId: number;
  public StartDate: Date;
  public EndDate: Date;
}
