import { DatePipe } from '@angular/common';

export class CarOfferModel {
  public Id: number;
  public Type: number;
  public Content: string;
  public CityId: number;
  public BrandId: number;
  public ModelId: number;
  public Variants: number[];
  public Dealers: number[];
  public Status: number;
  public Order: number;
  public StartDate: DatePipe;
  public EndDate: DatePipe;
  public CreatedDate: DatePipe;
  public CreatedDateStr: string;
  public ModifiedDate: DatePipe;
  public ModifiedDateStr: string;
  public EndDateStr: string;
  public StartDateStr: string;
  public CreatedBy: string;
  public ModifiedBy: string;
  public StatusName: string;
  public TypeName: string;
  public EndDateStrDDYYMM: string;
  public StartDateStrDDYYMM: string;
}


export class CarOfferGetListParam {
  public PageIndex: number;
  public PageSize: number;
  public TotalItem: number;
  public TotalPage: number;
  public CityId: number;
  public BrandId: number;
  public ModelId: number;
  public Status: number;
}

