export class ModelsModel {
  ModelId: number;
  BrandId: number;
  ModelName: string;
  ModelAlias: string;
}

export class SubModelsModel {
  SubModelId: number;
  BrandId: number;
  SubModelName: string;
  SubModelAlias: string;
  ParrentId: number;
}

export class BoxLinkDetailModel {
  Id: number;
  BoxLinkId: number;
  ModelId: number;
  Title: string;
  Url: string;
  SubmodelId: number;
  Year: number;
  ListModel: ModelsModel[] = [];
  ListSubModel: SubModelsModel[] = [];
}

export class BoxLinkModel {
  Id: number;
  BrandId: number;
  Title: string;
  Url: string;
  ListDetail: BoxLinkDetailModel[] = [];
}
