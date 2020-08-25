import { DatePipe } from '@angular/common';

export class AuthorModel {
  public id: number;
  public name: string;
  public alias: string;
  public status: number;
  public userid: number;
  public bio: string;
  public avatar: string;
  public email: string;
  public linkedin: string;
  public facebook: string;
  public whatsapp: string;
  public username: string;
  public statusname: string;
  public avatarCrop: string;
}

export class AuthorIndexModel {
  public ListUser: [];
  public DicAuthorProfileStatusName: [];
}

export class AuthorGetListCondition {
  public userid: number;
  public id: number;
  public status: number;
  public keyword: string;
  public pageindex: number;
  public pagesize: number;
  public lastmodifiedby: string;
  public startdateedit: Date;
  public enddateedit: Date;
}

export class AuthorChangeStatus {
  public userid: number;
  public id: number;
  public status: number;
  public username: string;
}
