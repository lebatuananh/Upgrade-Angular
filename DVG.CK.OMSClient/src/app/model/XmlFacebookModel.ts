import { DatePipe } from '@angular/common';

export class XMLFacebookConditions {
  public WebId: number;
  public BrandId: number;
  public ModelId: number;
  public CityId: number;
  public VipType: number;
  public SeconHandId: number;
  public BodyType: number;
  public CreatedUser: number;
  public PriceFrom: number;
  public PriceTo: number;
  public CreatedDateFrom: DatePipe;
  public CreatedDateTo: DatePipe;
  public YearFrom: number;
  public YearTo: number;
  public InputTitle: string;
  public ListGenTitleCondition: number[] = [];
  public ListGenTitleConditionJson: string;
  public CreatedDateFromStr: string;
  public CreatedDateToStr: string;
}
