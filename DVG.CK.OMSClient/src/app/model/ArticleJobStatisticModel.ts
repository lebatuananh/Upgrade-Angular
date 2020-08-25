import { DatePipe } from '@angular/common';

export class ArticleJobsStatisticGetListModel {
  public Assignee: string;
  public FromDate: DatePipe;
  public ToDate: DatePipe;
}

export class BarChartData {
  public label: string;
  public data: number[];

  constructor() {
    this.data = [];
  }
}
