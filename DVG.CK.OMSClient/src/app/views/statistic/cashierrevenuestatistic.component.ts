import { Component, OnInit, TemplateRef } from '@angular/core';
import { ngxLoadingAnimationTypes } from 'ngx-loading';
import { HttpService } from '../../services/http.service';
import { NotifierService } from 'angular-notifier';
import {
  CashierRevenueIndexModel,
  CashierRevenueSearchModel
} from '../../model/CashierRevenueIndexModel';
import { Title } from '@angular/platform-browser';
import { environment } from '../../../environments/environment';
import { NotifierTypeConst } from '../../utils/enums/notifier.enum';
import { NgxNotifierService } from 'ngx-notifier';
import { AppConfigs } from '../../../app/app.config';
import { DatePipe } from '@angular/common';
import {
  DateTimeAdapter,
  OWL_DATE_TIME_FORMATS,
  OWL_DATE_TIME_LOCALE
} from 'ng-pick-datetime';
import { MomentDateTimeAdapter } from 'ng-pick-datetime-moment';
export const FORMATS_DATE = {
  parseInput: 'DD/MM/YYYY',
  fullPickerInput: 'DD/MM/YYYY',
  datePickerInput: 'DD/MM/YYYY',
  timePickerInput: 'LT',
  monthYearLabel: 'MMM YYYY',
  dateA11yLabel: 'DD/MM/YYYY',
  monthYearA11yLabel: ' YYYY MMMM'
};

@Component({
  selector: 'app-cashierrevenuestatistic',
  templateUrl: './cashierrevenuestatistic.component.html',
  styleUrls: ['./cashierrevenuestatistic.component.scss'],
  providers: [
    {
      provide: DateTimeAdapter,
      useClass: MomentDateTimeAdapter,
      deps: [OWL_DATE_TIME_LOCALE]
    },
    { provide: OWL_DATE_TIME_FORMATS, useValue: FORMATS_DATE },
    DatePipe
  ]
})
export class CashierRevenueStatisticComponent implements OnInit {
  public IsShowIndexLoading = false;
  public IsShowGetListLoading = false;
  public loadingTemplate: TemplateRef<any>;
  public ngxLoadingAnimationTypes = ngxLoadingAnimationTypes;
  public IndexModel = new CashierRevenueIndexModel();
  public SearchModel = new CashierRevenueSearchModel();
  isShow = true;
  constructor(
    private httpService: HttpService,
    private notifier: NotifierService,
    private titleService: Title,
    private ngxNotifierService: NgxNotifierService,
    private datePipe: DatePipe,
  ) {
    this.titleService.setTitle(`Thống kê doanh thu theo thu ngân - ${environment.APP_TITLE}`);
    this.IsShowIndexLoading = true;
    this.httpService
      .DoPost(environment.APP_API_URL + '/statistic/index')
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.IndexModel = msg.Obj;
          this.SearchModel.FromDate = this.IndexModel.FromDateIndex;
          this.SearchModel.ToDate = this.IndexModel.ToDateIndex;
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
        this.IsShowIndexLoading = false;
        this.search();
      });
  }

  ngOnInit() {
  }

  public search() {
    this.ngxNotifierService.createToast('Test NGX', 'primary');
    this.IsShowGetListLoading = true;
    this.SearchModel.LstData = [];
    this.SearchModel.LstDataSouceType = [];
    this.SearchModel.LstDataPaymentType = [];
    this.SearchModel.LstDataProduct = [];
    this.SearchModel.LstCountProduct = [];
    this.SearchModel.LstCountBill = [];
    this.SearchModel.Type = this.SearchModel.Type == null ? 2 : this.SearchModel.Type
    this.isShow = this.SearchModel.Type == 0 ? true : false;
    if (this.SearchModel.FromDate) {
      this.SearchModel.FromDateStr = this.datePipe.transform(
        this.SearchModel.FromDate, AppConfigs.Ad_DateTimeFormatStringTimePicker);
    }
    if (this.SearchModel.ToDate) {
      this.SearchModel.ToDateStr = this.datePipe.transform(
        this.SearchModel.ToDate, AppConfigs.Ad_DateTimeFormatStringTimePicker);
    }
    this.httpService
      .DoPost(environment.APP_API_URL + '/statistic/getcashierrevenuestatistic', this.SearchModel)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.SearchModel.LstData = msg.Obj.LstData;
          this.SearchModel.LstDataSouceType = msg.Obj.LstDataSouceType;
          this.SearchModel.LstDataPaymentType = msg.Obj.LstDataPaymentType;
          this.SearchModel.LstDataProduct = msg.Obj.LstDataProduct;
          this.SearchModel.LstCountProduct = msg.Obj.LstCountProduct;
          this.SearchModel.LstCountBill = msg.Obj.LstCountBill;
          this.SearchModel.LstDate = msg.Obj.LstDate;
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
        this.IsShowGetListLoading = false;
      });
  }
  onChangeSourceType() {
    if (this.SearchModel.Type != 0)
      this.isShow = false;
    else
      this.isShow = true;
  }
}
