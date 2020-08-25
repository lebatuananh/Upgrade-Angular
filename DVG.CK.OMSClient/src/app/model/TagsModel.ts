export class TagsGetListConditions {
  public id: number;
  public keyword: string;
  public tagtype: number;
  public tagstatus: number;
  public pageindex: number = 1;
  public pagesize: number;
}

export class TagsModel {
  public id: number;
  public keyword: string;
  public alias: string;
  public type: number;
  public countarticle: number;
  public status: number;
  public url: string;
  public typename: string;
  public statusuame: string;
  public IsShowApproveButton: boolean;
  public IsShowDisApproveButton: boolean;
  public selected: boolean = false;
  public value: any;
  public display: string;
}

export class TagsGetListModel extends TagsGetListConditions {
  public Totalrecord: number;
  public Lstdata: TagsModel[] = [];
}

export class TagsChangeStatusModel {
  public lstId: number[] = [];
  public action: string;
  public lastmodifiedby: string;
  public tagstatus: number;
}
