import { Component, TemplateRef } from '@angular/core';
import { ngxLoadingAnimationTypes } from 'ngx-loading';
import { ActivatedRoute } from '@angular/router';
import { HttpService } from '../../services/http.service';
import { AuthenService } from '../../services/authen.service';
import { ResponseData } from '../../model/ResponseData';
const PrimaryWhite = '#ffffff';

@Component({
  selector: 'app-dashboard',
  templateUrl: 'logonsso.component.html'
})
export class LogonSSOComponent {
  public loadingTemplate: TemplateRef<any>;
  public primaryColour = PrimaryWhite;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;
  public Message = '';
  constructor(private activatedRoute: ActivatedRoute,
    private httpService: HttpService,
    public authenService: AuthenService) {
    const data = this.activatedRoute.snapshot.queryParams['data'];
    if (data === '101') {
      // tslint:disable-next-line:max-line-length
      this.Message = 'SSO account is not connected to Admin account.';
      return;
    }

    // call API
    this.authenService.doLoginSSO(data).subscribe(res => {
      if (res != null && res.Message.length > 0) {
        const obj = <ResponseData>res;
        this.Message = obj.Message;
      }
    });
  }

  public return() {
    return;
  }
}
