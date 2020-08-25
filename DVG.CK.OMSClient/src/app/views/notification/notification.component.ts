import { Component, OnInit, TemplateRef } from '@angular/core';
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
import { NotificationModel, NotificationIndexModel, NotificationGetListCondition } from '../../model/NotificationModel';

@Component({
  selector: 'app-notificationlist',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {
  ModalRef: BsModalRef;
  LoadingTemplate: TemplateRef<any>;
  NgxLoadingAnimationTypes = ngxLoadingAnimationTypes;
  IsShowLoader = false;
  IndexModel = new NotificationIndexModel();
  SearchModel = new NotificationGetListCondition();
  TotalItems = 0;
  TotalPage = 0;
  ListNotification = [];
  ListModelToBind = [];
  IsOpen = true;
  NotificationId = 0;

  constructor(
    private modalService: BsModalService,
    private httpService: HttpService,
    private notifier: NotifierService,
    private titleService: Title,
  ) {
    this.titleService.setTitle(`Notification Management - ${environment.APP_TITLE}`);
    this.IsShowLoader = !this.IsShowLoader;
    this.httpService
      .DoPost(environment.APP_API_URL + '/notification/index')
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.IndexModel = msg.Obj;
          this.search(1);
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
        this.IsShowLoader = !this.IsShowLoader;
      });
  }

  ngOnInit() {

  }

  search(page) {
    this.SearchModel.PageIndex = page;
    this.SearchModel.Keyword = this.SearchModel.Keyword && this.SearchModel.Keyword.length > 0 ? this.SearchModel.Keyword.trim() : this.SearchModel.Keyword;
    this.IsShowLoader = !this.IsShowLoader;
    this.httpService
      .DoPost(environment.APP_API_URL + '/notification/getlist', this.SearchModel)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.ListNotification = msg.Obj.ListDate;
          this.TotalItems = msg.Obj.Pager.TotalItem;
          this.TotalPage = msg.Obj.Pager.TotalPage;
          this.SearchModel.PageIndex = msg.Obj.Pager.CurrentPage;
          this.SearchModel.PageSize = msg.Obj.Pager.PageSize;
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
        this.IsShowLoader = !this.IsShowLoader;
      });
  }

  pageChange(event) {
    this.SearchModel.PageIndex = event;
    this.search(this.SearchModel.PageIndex);
  }

  onChangeIsOpen(event: boolean) {
    this.IsOpen = event;
  }

  onChangeBrand() {
    this.SearchModel.ModelId = null;
    this.ListModelToBind = [];
    if (this.SearchModel.BrandId > 0) {
      this.ListModelToBind = this.IndexModel.ListModel.filter(c => c.BrandId == this.SearchModel.BrandId);
    }
    this.search(1);
  }

  openModal(
    template: TemplateRef<any>,
    id: number
  ) {
    this.ModalRef = this.modalService.show(template, { class: 'modal-warning' });
    this.NotificationId = id;
  }

  deteleNotification(): void {
    this.ModalRef.hide();
    this.httpService
      .DoPost(environment.APP_API_URL + '/nitification/delete', null, { id: this.NotificationId })
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.notifier.notify(NotifierTypeConst.Success, msg.Title);
          this.search(1);
          this.ModalRef.hide();
          this.NotificationId = 0;
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });
  }


  //public getpositionbypage() {
  //  if (this.SearchModel.PlatformId !== null && this.SearchModel.PageId && this.SearchModel.PageId !== null) {
  //    const params = {
  //      platformId: this.SearchModel.PlatformId,
  //      pageId: this.SearchModel.PageId
  //    }
  //    this.httpService.DoPost(environment.APP_API_URL + '/bannerads/getpositionbypage', null, params).subscribe(p => {
  //      if (!p.Error) {
  //        this.IndexModel.ListBannerAdsPositionBinding = p.Obj;
  //      } else {
  //        this.notifier.notify(NotifierTypeConst.Error, p.Title);
  //      }
  //    });
  //  } else {
  //    this.IndexModel.ListBannerAdsPositionBinding = [];
  //    this.SearchModel.Position = null;
  //  }
  //}

  //public onChangePlatform() {
  //  this.SearchModel.PageId = null;
  //  this.IndexModel.ListBannerAdsPageBinding = [];
  //  if (this.SearchModel.PlatformId == 2) {
  //    this.IndexModel.ListBannerAdsPageBinding = this.IndexModel.ListBannerAdsPage.filter(x => {
  //      return (x.Key < 11);
  //    });
  //  }
  //  if (this.SearchModel.PlatformId == 1) {
  //    this.IndexModel.ListBannerAdsPageBinding = this.IndexModel.ListBannerAdsPage;
  //  }
  //  this.getpositionbypage();
  //}

  //public onChangePage() {
  //  this.SearchModel.Position = null;
  //  this.getpositionbypage();
  //}
}
