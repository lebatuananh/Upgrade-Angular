import { DatePipe } from '@angular/common';

export class DealershipModel {
  public Id: number;
  public Name: string;
  public DateContact: Date;
  public BrandId: number;
  public Type: number;
  public PrimaryPhoneNumber: string;
  public PhoneNumber: string;
  public Note: string;
  public Address: string;
  public CityId: number;
  public RegionId: number;
  public Link: string;
  public Status: number;
  public PosX: number;
  public PosY: number;
  public LastModifiedDate: Date;
  public LastModifiedBy: string;
  public Avatar: string;
  public Introduce: string;
  public RatePoint: number;
  public TextSearch: string;
  public PartnerType: number;
  public IsPublish: boolean;
  public CreatedDate: Date;
  public CreatedBy: string;
  public BrandIds: [] = [];
  public ModelIds: [] = [];
  public Email: string;
  public ModelId: number;
  // public StatusStr: string;
  // public TypeStr: string;
  // public BrandName: string;
  // public RegionName: string;
  // public CityName: string;
  // public DateContactStr: string;
  // public LastModifiedDateStr: string;
  // public CreatedDateStr: string;
  public AvatarCrop: string;
  public AvatarNoCrop: string;
  public ListLinkJson: string;
  public ListUserJson: string;
  public ListBrandJson: string;
  public ListModelJson: string;
  public ListImageJson: string;
}

export class DealershipIndexModel {
  public ListBrand: any[] = [];
  public ListModel: any[] = [];
  public ListRegion: any[] = [];
  public ListCity: any[] = [];
  public DicDealerShipsTypeName: [] = [];
  public DicDealerShipsStatusName: [] = [];
}

export class DealershipGetListCondition {
  public Id: number;
  public Name: string;
  public BrandId: number;
  public RegionId: number;
  public CityId: number;
  public Status: number;
  public Type: number;
  public PageIndex: number;
  public PageSize: number;
  public PartnerType: number;
}

export class DealershipEditModel {
  public ListBrand: any[] = [];
  public ListModel: any[] = [];
  public ListRegion: any[] = [];
  public ListCity: any[] = [];
  public DicDealerShipsTypeName: any[] = [];
  public DicDealerShipsStatusName: any[] = [];
  public Dealer: DealershipModel;
  public ListLink: any[] = [];
  public ListUser: any[] = [];
  public ListDealerBrand: any[] = [];
  public ListDealerModel: any[] = [];
  public ListImage: any[] = [];
}

export class DealerLinkModel {
  public Link: string;
  public Title: string;
  public Id: number;
  public WebId: number;
  public DealerShipsId: number;
  public Index: number;
}

