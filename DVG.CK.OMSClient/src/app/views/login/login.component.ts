import { Component, Input, TemplateRef } from '@angular/core';
import { SystemUserLoginModel } from '../../model/SystemUserLoginModel';
import { ngxLoadingAnimationTypes } from 'ngx-loading';
import { AuthenService } from '../../services/authen.service';
import { ReturnStatement } from '@angular/compiler';
import { Title } from '@angular/platform-browser';
import { environment } from '../../../environments/environment';
const PrimaryWhite = '#ffffff';


@Component({
  selector: 'app-dashboard',
  templateUrl: 'login.component.html'
})
export class LoginComponent {
  @Input() _systemUserLoginModel: SystemUserLoginModel;
  public loadingTemplate: TemplateRef<any>;
  public primaryColour = PrimaryWhite;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;

  constructor(public authenService: AuthenService,
    private titleService: Title,
  ) {
    this.titleService.setTitle(`Login - ${environment.APP_TITLE}`);
    this._systemUserLoginModel = new SystemUserLoginModel();
    setTimeout(() => {
      this.authenService.tryReload();
    }, 1000);
  }

  public login() {
    this.authenService.doLogin(this._systemUserLoginModel).subscribe();
  }

  public goToSSO() {
    this.authenService.doGoToSSO();
  }

  public return() {
    return;
  }
}
