export class ArticleStatisticModel {
  public CreatedBy: string;
  public DayGet: Date;
  public DayGetStr: string;
  public TotalNews: number;
  public ArticleType: number;
}
export class ArticleStatisticIndexModel {
  public ListUser: any[] = [];
}
export class ArticleStatisticGetListModel {
  public ListData: ArticleStatisticModel[] = [];
  public ListUser: any[] = [];
  public FromDate: Date;
  public FromDateStr: string;
  public ToDate: Date;
  public ToDateStr: string;
  public FromMonth: number;
  public ToMonth: number;
  public Year: number = new Date().getFullYear();
  public StatisticType: string;
  public CreatedBy: string;
}
