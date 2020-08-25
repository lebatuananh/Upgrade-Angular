export class ArticleCommentGetListModel {
  public webid: number;
  public id: number;
  public status: number;
  public havecomment: boolean;
  public pageindex: number;
  public pagesize: number;
  public commenttype: number;
  public brandid: number;
  public modelid: number;
  public submodelid: number;
}

export class ArticleCommentIndexModel {
  public ListBrand: any[] = [];
  public ListModel: any[] = [];
  public ListSubModel: any[] = [];
  public ListWeb: any[] = [];
  public CommentType: number;
  public IsHaveBrandAndModel: boolean;
  public DicArticleCommentStatusName: any[] = [];
}

export class ArticleCommentChangeStatus {
  public webid: number;
  public lstId: any[] = [];
  public action: string;
  public lastmodifiedby: string;
  public status: number;
}

export class ArticleCommentEdit {
  public webid: number;
  public id: number;
  public title: string;
  public content: number;
}
