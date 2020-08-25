export class PostingPublishedStatisticModel {
  public UserId: number;
  public DayGet: Date;
  public DayGetStr: string;
  public TotalNews: number;
  public WebId: number;
}
export class PostingPublishedStatisticIndexModel {
  public ListUser: any[] = [];
}
export class PostingPublishedStatisticGetListModel {
  public ListData: PostingPublishedStatisticModel[] = [];
  public ListUser: any[] = [];
  public FromDate: Date;
  public ToDate: Date;
  public FromMonth: number;
  public ToMonth: number;
  public Year: number = new Date().getFullYear();
  public StatisticType: string;
  public UserId: number;
}
