import { DatePipe } from "@angular/common";

export class UserModel {
  public UserId: number;
  public UserName: string;
  public Password: string;
  public Email: string;
  public Mobile: string;
  public FullName: string;
  public Avatar: string;
  public Status: number;
  public StatusBack: string;
  public CreatedDate: Date;
  public LastupdatedUser: number;
  public CreatedDateStr: string;
  public Gender: boolean;
  public Address: string;
  public PasswordQuestion: string;
  public PasswordAnswer: string;
  public UserType: number;
  public BirthdayStr: string;
  public Birthday: Date;
}

export class UsersSearchModel {
  public PageIndex: number;
  public PageSize: number;
  public Keyword: string;
  public UserType: number;
}

export class ResetPassWordModel {
  public UserId: number;
  public FullName: string;
  public Email: string;
  public NewPassword: string;
}

export class UsersActionModel {
  public UserId: number;
  public UserName: string;
  public FullName: string;
  public Email: string;
  public BirthdayStr: string;
  public Mobile: string;
  public Avatar: string;
  public UserType: number;
  public Gender: number;
  public Address: string;

}
