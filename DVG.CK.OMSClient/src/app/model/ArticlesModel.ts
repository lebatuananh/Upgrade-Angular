import { DatePipe } from '@angular/common';
import { AuthorModel } from './AuthorModel';

export class ArticleModel {
  public id: number;
  public title: string;
  public titlespotlight: string;
  public sapo: string;
  public longcontent: string;
  public shortcontent: string;
  public avatar: string;
  public avatarCrop: string;
  public type: number;
  public status: number;
  public isPublish: boolean;
  public createdDate: DatePipe;
  public lastModifiedDate: DatePipe;
  public publishedDate: DatePipe;
  public publisheddatestr: string;
  public createdBy: string;
  public lastModifiedBy: string;
  public publishedBy: string;
  public sourceurl: string;
  public sourcename: string;
  public metatitle: string;
  public metakeyword: string;
  public metadescription: string;
  public categoryid: number;
  public textsearch: string;
  public havevideo: boolean;
  public isdmca: boolean;
  public displayType: number;
  public isSelected = false;
  public IsGalleryImage: boolean;
  public IsGalleryVideo: boolean;
  public IsHaveBrandAndModel: boolean;
  public IsHaveHeadings: boolean;
  public IsHaveMultiBrandAndModel: boolean;
  public IsShowDeleteButton: boolean;
  public IsShowDisApprovedButton: boolean;
  public IsShowNotApprovedButton: boolean;
  public IsShowSaveAndPublishButton: boolean;
  public IsShowSaveButton: boolean;
  public IsShowShortContent: boolean;
  public lsttag: any;
  public ListCarInfo: any;
  public ListModel: any;
  public ListBrand: any;
  public ListBodyType: any;
  public DicNewCarStatus: any;
  public lstyear: any;
  public ListGallery: any;
  public videoUrl: string;
  public videoAlt: string;
  public IsUsingTeamName: boolean;
  public AuthorInfo: AuthorModel;
  public authorid: number;
  public isgoogleamp: boolean;
  public publishwithoutreview: boolean;
  public DicAuthorTeamName: any[];
  public titleseo: string;
  public SlideTypeName: any[];
  public ListDealerships: any[];
  public ArticleExtend: ArticleExtendModel;
  public LinkDetail: string;
  public avatarHighlight: string;
  public AvatarHighlightCrop: string;
  public avatarHighlightWidth: number;
  public avatarHighlightHeight: number;
  public TypeName: string;
  public viewcount: number;
}

export class ArticleSearchModel {
  public id: number;
  public keyword: string;
  public createdBy: string;
  public lastModifiedBy: string;
  public startDateEdit: DatePipe;
  public endDateEdit: DatePipe;
  public startDateCreate: DatePipe;
  public endDateCreate: DatePipe;
  public status: number;
  public type: number;
  public pageIndex: number;
  public pageSize: number;
  public displayType: number;
  public brandId: number;
  public modelId: number;
  public carinfoStatus: number;
  public authorid: number;
}

export class ArticlesGetListModel {
  public lstData: ArticleModel[] = [];
}

export class ArticleChangeStatusModel {
  public lstId: number[] = [];
  public action: string;
}

export class ArticlesActionHistoryModel {
  public id: number;
  public createdby: string;
  public createddate: DatePipe;
  public type: number;
  public createddatestr: string;
  public typestr: string;
}

export class ArticleImageModel {
  public UrlCrop: string;
  public UrlNoCrop: string;
  public alt: string;
  public articleid: number;
  public id: number;
  public priority: number;
  public slidetype: number;
  public url: string;
}

export class ArticleExtendModel {
  public articleid: number;
  public brandid: number;
  public modelid: number;
  public startdate: Date;
  public EndDateStr: string;
  public enddate: Date;
  public StartDateStr: string;
  public dealershipsid: number;
}
