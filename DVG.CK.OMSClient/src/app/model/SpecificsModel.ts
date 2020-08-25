import { DatePipe } from '@angular/common';

export class Specgetlistconditions {
  public Id: number;
  public BrandId: number;
  public ModelId: number;
  public SubModelId: number;
  public ProductedYear: number;
  public KeyWord: string;
  public StartDateEdit: DatePipe;
  public EndDateEdit: DatePipe;
  public StartDateCreate: DatePipe;
  public EndDateCreate: DatePipe;
  public LastModifiedBy: DatePipe;
  public IsPrimary: number;
  public PageIndex: number;
  public PageSize: number;
  public Status: number;
}

export class SpecGetByIdModel {
  public Id: number;
  public Name: string;
  public BrandId: number;
  public ModelId: number;
  public SubmodelId: number;
  public ProductedYear: number;
  public BodyType: number;
  public TransmissionId: number;
  public Status: number;
  public CreateDate: DatePipe;
  public LastupdatedDate: DatePipe;
  public CreatedUser: number;
  public LastupdatedUser: number;
  public Price: number;
  public IsPrimary: boolean;
  public BrandName: string;
  public ModelName: string;
  public SubmodelName: string;
  public Description: string;
  public TransmissionName: string;
  public BodyTypeStr: string;
  public lstTech: SpecEntity[] = [];
  public lstGroupName: string[] = [];
  public IsClone: boolean;
  public ListTransmissions: [] = [];
  public ListBodyType: [] = [];
  public ListBrand: any[] = [];
  public ListModel: any[] = [];
  public ListSubModel: any[] = [];
}

export class SpecEntity {
  public TechId: number;
  public TechName: string;
  public TechValue: string;
  public GroupId: number;
  public GroupName: string;
}
