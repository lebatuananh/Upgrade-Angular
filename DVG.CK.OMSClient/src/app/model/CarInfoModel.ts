export class CarInfoModel {
    public id: number;
    public articleid: number;
    public brandid: number;
    public brandname: string;
    public modelid: number;
    public modelname: string;
    public status: number;
    public statusname: string;
    public bodytype: number;
    public bodytypename: string;
    public productionyear: number;
    public pricename: string;
    public price: number;
    public StatusOfCarInfo: StatusOfCarInfo;
}

export class CarModelModel {
    public BrandId: number;
    public ModelAlias: string;
    public ModelId: number;
    public ModelName: string;
}

export class StatusOfCarInfo {
    public IsNormal: boolean;
    public IsUpComing: boolean;
}