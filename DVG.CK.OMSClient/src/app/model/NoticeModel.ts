import { DatePipe } from '@angular/common';
export class GetListNoticeParam {
  public Keyword: string;
  public Status: number;
  public PageIndex: number = 1;
  public PageSize: number;
}

export class NoticeModel {
  public Id: number;
  public Title: string;
  public Content: string;
  public CreatedBy: string;
  public UpdatedBy: string;
  public Status: number;
  public Ordinal: number;
  public UpdatedDate: DatePipe;
  public CreatedDate: DatePipe;
  public CreatedDateStr: string;
  public UpdatedDateStr: string;
  public StatusStr: string;
}

export class ListNoticeModel extends GetListNoticeParam {
  public Totalrecord: number;
  public Lstdata: NoticeModel[] = [];
}

