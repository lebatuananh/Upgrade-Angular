import { DatePipe } from '@angular/common';
import { MemberInfo } from './MemberModel';

export class UserCrawlerInfo {
  public Id: number;
  public DisplayName: string;
  public Address: string;
  public Phone: string;
  public Email: string;
  public ContactStatus: number;
}
export class Membershipcrawl extends UserCrawlerInfo {
  public Note: string;
  public Status: number;

}
export class UserCrawlerGetListConditions {
  public PageIndex: number;
  public PageSize: number;
  public Keyword: string;
  public Islock: boolean;
  public Id: number;
  public StartDate: Date;
  public EndDate: Date;
  public MobilePrefix: string;
  public MobilePrefixName: string;
  public ListMobilePrefix: string[];
  public Status: number;
  public ContactStatus: number;
  public ProductCountConditions: string;
  public ListShowColumnExcel: string[];
  public ListProductCountConditions: string[];
  public RemoveIncorrectPhone: boolean;
  public RemoveDuplicatePhone: boolean;
}

export class ProductChangeToCusGetListConditions {
  public UserId: number;
  public PageIndex: number;
  public PageSize: number;
  public WebId: number;
}

export class UsercrawlChangeToCusSave {
  public ListProductIdSelected: number[];
  public ListWebIdSelected: number[];
  public IsSelectAllPages: boolean;
  public ProducToWebType: number;
  public Member: MemberInfo;
  public UserCrawl: Membershipcrawl;

  constructor() {
    this.ListProductIdSelected = [];
    this.ListWebIdSelected = [];
    this.IsSelectAllPages = false;
    this.UserCrawl = new Membershipcrawl();
    this.Member = new MemberInfo();
  }
}
