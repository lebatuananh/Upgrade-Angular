import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { NotificationModel } from '../../model/NotificationModel';
import { NotifierService } from 'angular-notifier';
import { NotifierTypeConst } from '../../utils/enums/notifier.enum';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { HttpService } from '../../services/http.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ngxLoadingAnimationTypes } from 'ngx-loading';
import { Title } from '@angular/platform-browser';
import { DatePipe } from '@angular/common';
import { DateTimeAdapter, OWL_DATE_TIME_FORMATS, OWL_DATE_TIME_LOCALE } from 'ng-pick-datetime';
import { MomentDateTimeAdapter } from 'ng-pick-datetime-moment';

export const FORMATS_DATE = {
  parseInput: 'DD/MM/YYYY HH:mm',
  fullPickerInput: 'DD/MM/YYYY HH:mm',
  datePickerInput: 'DD/MM/YYYY',
  timePickerInput: 'LT',
  monthYearLabel: 'M YYYY',
  dateA11yLabel: 'DD/MM/YYYY',
  monthYearA11yLabel: 'M YYYY'
};
@Component({
  selector: 'app-add-notification',
  templateUrl: './addnotification.component.html',
  styleUrls: ['./addnotification.component.css'],
  providers: [
    DatePipe,
    { provide: DateTimeAdapter, useClass: MomentDateTimeAdapter, deps: [OWL_DATE_TIME_LOCALE] },
    { provide: OWL_DATE_TIME_FORMATS, useValue: FORMATS_DATE }
  ]
})
export class AddNotificationComponent implements OnInit {
  @Input()
  notificationModel: NotificationModel;
  ViewDomain: string;
  SizeBanner: string;
  public loading = false;
  public loadingTemplate: TemplateRef<any>;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;
  public primaryColour = '#ffffff';
  listModelToBind: any[] = [];
  listBrand: any[] = [];
  listModel: any[] = [];
  listDeliveryType: any[] = [];
  minDate = new Date();
  config: Object = {
    hasAllCheckBox: true,
    hasFilter: true,
    hasCollapseExpand: true,
    decoupleChildFromParent: false,
    maxHeight: 500
  };

  constructor(private notifier: NotifierService,
    private httpService: HttpService,
    private route: ActivatedRoute,
    private router: Router,
    private titleService: Title,
    private datePipe: DatePipe,
  ) {
    this.notificationModel = new NotificationModel();
  }

  ngOnInit() {
    this.notificationModel = new NotificationModel();
    this.loading = !this.loading;
    this.httpService
      .DoPost(environment.APP_API_URL + '/notification/add')
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.listBrand = msg.Obj.ListBrand;
          this.listModel = msg.Obj.ListModel;
          this.listDeliveryType = msg.Obj.ListDeliveryType;
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
        this.loading = !this.loading;
      });
  }

  public save(): Observable<any> {
    this.loading = !this.loading;
    return this.httpService.DoPost(environment.APP_API_URL + '/notification/save', this.notificationModel, null);
  }

  public doSend() {
    if (confirm('Are you sure?')) {
      this.save().subscribe(p => {
        if (!p.Error) {
          this.loading = !this.loading;
          this.notifier.notify(NotifierTypeConst.Success, "Sent successfully!");
          setTimeout(() => {
            this.router.navigate(['/notification']);
          }, 1000);
        } else {
          this.loading = !this.loading;
          this.notifier.notify(NotifierTypeConst.Error, "Send failed!");
        }
      });
    }
  }

  onChangeBrand() {
    this.notificationModel.ModelId = null;
    this.listModelToBind = [];
    if (this.notificationModel.BrandId > 0) {
      this.listModelToBind = this.listModel.filter(c => c.BrandId == this.notificationModel.BrandId);
    }
  }

  txtDestinationLink_paste(e) {
    e.preventDefault();
    var pastedText = '';

    if (e.clipboardData && e.clipboardData.getData) {// Standards Compliant FIRST!
      pastedText = e.clipboardData.getData('text/plain');
      this.notificationModel.DestinationLink = pastedText.trim();
    }

  }
  txtImageLink_paste(e) {
    e.preventDefault();
    var pastedText = '';

    if (e.clipboardData && e.clipboardData.getData) {// Standards Compliant FIRST!
      pastedText = e.clipboardData.getData('text/plain');
      this.notificationModel.Image = pastedText.trim();
    }

  }
  public return() {
    return;
  }

  cancelAdd() {
    if (confirm('Are you sure?')) {
      this.router.navigate(['/notification']);
    }
  }
}
