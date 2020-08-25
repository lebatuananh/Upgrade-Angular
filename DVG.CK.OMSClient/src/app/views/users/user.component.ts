import { Component, OnInit, TemplateRef, ViewChild, ComponentRef } from '@angular/core';
import { environment } from '../../../environments/environment';
import { NotifierTypeConst } from '../../utils/enums/notifier.enum';
import { HttpService } from '../../services/http.service';
import { NotifierService } from 'angular-notifier';
import { TagsGetListModel, TagsModel, TagsChangeStatusModel } from '../../model/TagsModel';
import {
  BsModalRef,
  BsModalService
} from 'ngx-bootstrap/modal';
import { Title } from '@angular/platform-browser';
import { ngxLoadingAnimationTypes } from 'ngx-loading';
import { Pager } from '../../model/ResponseData';
import { ActivatedRoute, Router } from '@angular/router';
import { RolesEnum } from '../../utils/enums/roles.enum';
import { UsersSearchModel, ResetPassWordModel, UsersActionModel, UserModel } from '../../model/UserModel';

@Component({
  selector: 'app-users',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UsersComponent implements OnInit {
  LoadingTemplate: TemplateRef<any>;
  NgxLoadingAnimationTypes = ngxLoadingAnimationTypes;
  RolesEnum = RolesEnum;
  IndexModel: any;
  SearchUser = new UsersSearchModel();
  ListData = [];
  IsShowLoader = false;
  page = new Pager();
  IsOpen = false;
  ResetPassWordModel = new ResetPassWordModel();
  ListUserType = {
    "ListStatus": [],
    "CurrentType": 0
  };
  CatePermissionModel = {
    'UserId': 0,
    'Username': "",
    'ListPermistionSelected': [],
    "ListPermistion": []
  };
  UsersStatus = {
    "Active": 1,
    "Lock": 0
  };
  modalRef: BsModalRef;
  id: number;
  message: string;

  UsersActionModel = new UserModel();
  PatternEmail = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,5})$/;
  PatternPhone = /^[0-9]{9,12}$/;
  PatternUserName = /^[a-zA-Z0-9-_]{6,}$/;
  constructor(
    private modalService: BsModalService,
    private httpService: HttpService,
    private notifier: NotifierService,
    private titleService: Title,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.titleService.setTitle(`Quản lý tài khoản - ${environment.APP_TITLE}`);
    this.httpService
      .DoPost(environment.APP_API_URL + '/users/getlistusertype', null, null)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.ListUserType.ListStatus = msg.Obj.UsersTypeList;
          this.ListUserType.CurrentType = null;
        } else {
          this.ListUserType.ListStatus = [];
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });
  }

  ngOnInit() {
    this.searchUsers(1);
  }

  pageChange(event) {
    this.page.CurrentPage = event;
    this.searchUsers(event);
  }


  onChangeIsOpen(event: boolean) {
    this.IsOpen = event;
  }

  searchUsers(page) {
    this.IsShowLoader = !this.IsShowLoader;
    this.ListData = [];
    this.SearchUser.PageIndex = page;
    this.SearchUser.Keyword = this.SearchUser.Keyword != null ? this.SearchUser.Keyword.trim() : '';
    this.SearchUser.UserType = this.ListUserType.CurrentType == null ? 0 : this.ListUserType.CurrentType;
    this.httpService
      .DoPost(environment.APP_API_URL + '/users/search', this.SearchUser, null)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.ListData = msg.Obj.Data;
          this.page = msg.Obj.Pager;
          this.SearchUser.PageIndex = msg.Obj.Pager.CurrentPage;
          this.SearchUser.PageSize = msg.Obj.Pager.PageSize;
        } else {
          this.ListData = [];
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
        this.IsShowLoader = !this.IsShowLoader;
      });
  }
  public openModalConfirm(template: TemplateRef<any>, item: any, message: string) {
    this.modalRef = this.modalService.show(template, { class: 'modal-xs' });
    this.id = item.UserId;
    this.message = message;    
  }
  public confirm(): void {
    this.modalRef.hide();
    var param = {
      userId: this.id
    }
    this.httpService
      .DoPost(environment.APP_API_URL + '/users/updatestatus', null, param)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.notifier.notify(NotifierTypeConst.Success, msg.Title);
          this.searchUsers(this.page.CurrentPage);
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });
  }


  public openResetPassword(template: TemplateRef<any>, item: any) {
    this.ResetPassWordModel.UserId = item.UserId;
    this.ResetPassWordModel.FullName = item.FullName;
    this.ResetPassWordModel.Email = item.Email;
    this.ResetPassWordModel.NewPassword = '';
    this.modalRef = this.modalService.show(template, { class: 'modal-md' });
  }

  public resetPassword(): void {
    if (this.ResetPassWordModel.NewPassword.trim() == '') {
      this.notifier.notify(NotifierTypeConst.Error, 'Vui lòng nhập password mới!');
      return;
    } else {
      if (this.ResetPassWordModel.NewPassword.trim().length < 6 || this.ResetPassWordModel.NewPassword.trim().length > 255) {
        this.notifier.notify(NotifierTypeConst.Error, 'Vui lòng nhập password mới có tối thiểu 6 ký tự và tối đa 255 ký tự!');
        return;

      }
    }
    var param = {
      userId: this.ResetPassWordModel.UserId,
      passWord: this.ResetPassWordModel.NewPassword
    }
    this.httpService
      .DoPost(environment.APP_API_URL + '/users/resetpassword', null, param)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.notifier.notify(NotifierTypeConst.Success, msg.Title);
          this.modalRef.hide();
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });
  }

  public autoGeneratePassword() {
    return this.httpService
      .DoPost(environment.APP_API_URL + '/users/generatepassword', null, null)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.ResetPassWordModel.NewPassword = msg.Obj;
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });
  }


  public openCatePermission(template: TemplateRef<any>, item: any) {
    this.CatePermissionModel.UserId = item.UserId;
    this.CatePermissionModel.Username = item.UserName;
    this.ListUserType.CurrentType = item.UserType;
    
    this.modalRef = this.modalService.show(template, { class: 'modal-md' });
  }

  public changeCatePermission() {
    if (this.ListUserType.CurrentType == null || this.ListUserType.CurrentType <= 0) {
      this.notifier.notify(NotifierTypeConst.Error, 'Vui lòng chọn phân quyền !');
      return;
    }
    var param = {
      userId: this.CatePermissionModel.UserId,
      userType: this.ListUserType.CurrentType
    }
    return this.httpService
      .DoPost(environment.APP_API_URL + '/users/UpdateCatePermission', null, param)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.notifier.notify(NotifierTypeConst.Success, 'Phân quyền thành công!');
          this.searchUsers(this.page.CurrentPage);
          this.modalRef.hide();
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });
  }

  public openUpdate(template: TemplateRef<any>, id: number) {
    this.modalRef = this.modalService.show(template, { class: 'modal-lg' });
    this.UsersActionModel.UserId = id;
    var param = {
      userId: id
    }
    this.httpService
      .DoPost(environment.APP_API_URL + '/users/getuserbyid', null, param)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.UsersActionModel = msg.Obj;
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });
  }

  public doUpdate() {
    if (this.UsersActionModel.UserName == null || this.UsersActionModel.UserName == "") {
      this.notifier.notify(NotifierTypeConst.Error, "Tên đăng nhập không được để trống ");
      return;
    } else {
      if (!this.PatternUserName.test(this.UsersActionModel.UserName)) {
        this.notifier.notify(NotifierTypeConst.Error, "Tên đăng nhập phải từ 6 ký tự trở lên và không được chứa ký tự đặc biệt và chữ có dấu.");
        return;
      }
    }
    if (this.UsersActionModel.UserType == null || this.UsersActionModel.UserType <= 0) {
      this.notifier.notify(NotifierTypeConst.Error, "Loại tài khoản không được để trống.");
      return;
    }
    if (this.UsersActionModel.FullName == null || this.UsersActionModel.FullName == "") {
      this.notifier.notify(NotifierTypeConst.Error, "Họ và tên không được để trống.");
      return;
    }
    if (this.UsersActionModel.Email == null || this.UsersActionModel.Email == "") {
      this.notifier.notify(NotifierTypeConst.Error, "Email không được để trống ");
      return;
    } else {
      if (!this.PatternEmail.test(this.UsersActionModel.Email)) {
        this.notifier.notify(NotifierTypeConst.Error, "Email không đúng định dạng.");
        return;
      }
    }
    if (this.UsersActionModel.Mobile != null && this.UsersActionModel.Mobile != "") {
      if (!this.PatternPhone.test(this.UsersActionModel.Mobile)) {
        this.notifier.notify(NotifierTypeConst.Error, "Số điện thoại không đúng định dạng.");
        return;
      }
    } 

    this.httpService
      .DoPost(environment.APP_API_URL + '/users/update', this.UsersActionModel, null)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          if (this.UsersActionModel.UserId == 0)
            this.notifier.notify(NotifierTypeConst.Success, 'Thêm thành công!');
          else
            this.notifier.notify(NotifierTypeConst.Success, 'Cập nhật thành công!');

          this.searchUsers(this.page.CurrentPage);
          this.modalRef.hide();
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });
  }
}
