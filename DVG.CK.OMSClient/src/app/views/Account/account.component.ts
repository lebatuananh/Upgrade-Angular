import { Component, OnInit, TemplateRef, ViewChild, ComponentRef } from '@angular/core';
import { environment } from '../../../environments/environment';
import { NotifierTypeConst } from '../../utils/enums/notifier.enum';
import { HttpService } from '../../services/http.service';
import { NotifierService } from 'angular-notifier';
import {
  BsModalRef,
  BsModalService
} from 'ngx-bootstrap/modal';
import { Title } from '@angular/platform-browser';
import { ngxLoadingAnimationTypes } from 'ngx-loading';

import { ChangePasswordModel } from '../../model/AccountModel';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  ChangePasswordModel = new ChangePasswordModel();
  
  constructor(
    private modalService: BsModalService,
    private httpService: HttpService,
    private notifier: NotifierService,
    private titleService: Title,
  ) {
    this.titleService.setTitle(`Đổi mật khấu - ${environment.APP_TITLE}`);
  }

  ngOnInit() {      
  }

  doChangePassword() {
    //if (this.ChangePasswordModel.CurrentPassword == null || this.ChangePasswordModel.CurrentPassword == "") {
    //  this.notifier.notify(NotifierTypeConst.Error, "Vui lòng nhập mật khẩu hiện tại !");
    //  return;
    //}
    this.httpService
      .DoPost(environment.APP_API_URL + '/account/changepassword', this.ChangePasswordModel, null)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.notifier.notify(NotifierTypeConst.Success, 'Đổi mật khẩu thành công!');
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });
  }
}
