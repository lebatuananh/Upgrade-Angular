export class PostingStatisticModel {
  public TotalNews: number;
  public DayGet: Date;
  public DayGetStr: string;
  public WebId: number;
}
export class PostingStatisticIndexModel {
  public ListWeb: any[] = [];
  public StartDateIndex: Date;
  public EndDateIndex: Date;
}
export class PostingStatisticGetListModel {
  public ListData: PostingStatisticModel[] = [];
  public ListWeb: any[] = [];
  public WebId: number;
  public StartDate: Date;
  public EndDate: Date;
}
