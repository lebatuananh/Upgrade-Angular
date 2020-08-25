export class CitiesChangePriority {
  public Id: number;
  public LastUpdatedUser: number;
  public Priority: number;
  public IsFeatured: boolean;
}

export class CitiesGetListConditions {
  public pageindex: number;
  public pagesize: number;
  public keyword: string;
  public regionid: number;
}

export class CitiesGetListResult {
  public CityID: number;
  public CityName: string;
  public CityAlias: string;
  public RegionName: string;
  public CityNameEnglish: string;
  public Priority: number;
  public Createdatestr: string;
  public Lastupdatedatestr: string;
  public IsFeatured: boolean;
}

export class CitiesGetListResultModel extends CitiesGetListConditions {
  public totalrecord: number;
  public lstdata: CitiesGetListResult[] = [];
}

export class CitiesChangePriorityModel {
  public ListChangePriority: CitiesChangePriority[] = [];
}
