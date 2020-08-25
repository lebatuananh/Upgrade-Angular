import { DatePipe } from '@angular/common';

export class ArticleJobInfo {
  public id: number;
  public title: string;
  public articletype: number;
  public articleid: number;
  public status: number;
  public wordcount: number;
  public deadlinestr: DatePipe;
  public createddatestr: DatePipe;
  public sidenote: string;
  public assigneenote: string;
  public assignee: string;
  public createdby: string;
  public isread: boolean;
  public isassigntome: boolean;
  public articletypename: string;
  public statusstr: boolean;
  public IsShowAssignToMeButton: boolean;
  public IsShowSaveButton: boolean;
  public ListArticleType: any[];
  public articlename: string;
}

export class ArticleJobsGetListModel {
  public PageIndex: number;
  public PageSize: number;
  public Keyword: string;
  public Assignee: string;
  public CreatedBy: string;
  public Status: number;
  public Tab: number;
  public ToDeadlineDate: DatePipe;
  public FromDeadlineDate: DatePipe;
  public ToCreatedDate: DatePipe;
  public FromCreatedDate: DatePipe;
  public FromEditDate: DatePipe;
  public ToEditDate: DatePipe;
}

