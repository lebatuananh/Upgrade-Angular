import { DatePipe } from '@angular/common';

export class BrandChangePriority {
  public BrandId: number;
  public LastUpdatedUser: number;
  public Priority: number;
}

export class BrandGetListConditions {
  public pageindex: number;
  public pagesize: number;
  public keyword: string;
}

export class BrandGetListResult {
  public BrandId: number;
  public BrandName: string;
  public BrandGroup: number;
  public IsActive: boolean;
  public BrandOrder: number;
  public MetaTitle: string;
  public DateTime: DatePipe;
  public BrandAlias: string;
  public Priority: number;
  public Createdatestr: string;
  public Lastupdatedatestr: string;
}

export class BrandGetListResultModel extends BrandGetListConditions {
  public totalrecord: number;
  public lstdata: BrandGetListResult[] = [];
}

export class BrandChangePriorityModel {
  public ListChangePriority: BrandChangePriority[] = [];
}
