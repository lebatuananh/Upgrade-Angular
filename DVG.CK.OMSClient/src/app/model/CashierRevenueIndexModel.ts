export class CashierRevenueIndexModel {
  public FromDateIndex: Date;
  public ToDateIndex: Date;
  public ListRevenueType: any[] = [];
}

export class CashierRevenueSearchModel {
  public LstData: CashierRevenueStatisticModel[];
  public LstDataSouceType: CashierRevenueStatisticModel[];
  public LstDataPaymentType: CashierRevenueStatisticModel[];
  public LstDataProduct: CashierRevenueStatisticModel[];
  public LstCountProduct: CashierRevenueStatisticModel[];
  public LstCountBill: CashierRevenueStatisticModel[];
  public LstDate: Date[];
  public FromDate: Date;
  public ToDate: Date;
  public FromDateStr: string;
  public ToDateStr: string;
  public Type: number = 0;
}

export class CashierRevenueStatisticModel {
  public CashierName: string;
  public TotalStr: string;
  public lstDay: CashierRevenueStatisticByDayModel[];
}

export class CashierRevenueStatisticByDayModel {
  public Day: Date;
  public Revenue: number;
  public RevenueStr: string;
}