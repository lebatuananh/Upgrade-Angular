export class DealershipFeaturedIndexModel {
  public ListWeb: any[];
  public PositionId: number;
}

export class DealershipFeaturedGetByConditionModel {
  public WebId: number;
  public PositionId: number;
}

export class DealershipFeaturedAddModel {
  public WebId: number;
  public PositionId: number;
  public DealerId: number;
  public Priority: number;
}

export class DealershipFeaturedModel {
  public DealerShipsId: number;
  public PositionId: number;
  public Name: string;
  public Priority: number;
  public PrimaryPhoneNumber: number;
  public Address: number;
  public Avatar: number;
}

export class DealerFeaturedChangePriority {
  public webId: number;
  public listDealerPosition: DealershipFeaturedModel[];
}
