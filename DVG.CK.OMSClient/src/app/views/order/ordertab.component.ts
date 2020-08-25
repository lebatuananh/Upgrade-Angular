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
import { OrderIndexModel, OrderSearchModel } from '../../model/OrderModel';
import { Pager } from '../../model/ResponseData';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { OrderStatusEnum } from '../../utils/enums/order.enum';
import { OrderListComponent } from './orderlist.component';
import { RolesEnum } from '../../utils/enums/roles.enum';
import { AuthenService } from '../../services/authen.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-ordertab',
  templateUrl: './ordertab.component.html',
  styleUrls: ['./ordertab.component.css']
})
export class OrderTabComponent implements OnInit {
  IndexModel = new OrderIndexModel();
  public sub: any;
  tab: any;
  SearchOrder = new OrderSearchModel();
  ListData = [];
  IsShowLoader = false;
  page = new Pager();
  @ViewChild('staticTabs') staticTabs: TabsetComponent;
  @ViewChild('orderlist') orderlist: OrderListComponent;
  RolesEnum = RolesEnum;
  Role: any;
  Count = {
    CountTab : 0,
    CountTab1 : 0,
    CountTab2 : 0,
    CountTab3 : 0,
    CountTab4 : 0,
    CountTab5 : 0,
    CountTab6 : 0,
    CountTab7: 0,
    CountTab8: 0,
    CountTab9: 0,
    CountTab10: 0,
    CountTab11: 0,
  }
  constructor(
    private modalService: BsModalService,
    private httpService: HttpService,
    private notifier: NotifierService,
    private titleService: Title,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.titleService.setTitle(`Quản lý đơn hàng - ${environment.APP_TITLE}`);
    this.Role = +AuthenService.getRoles();
    if (this.Role === RolesEnum.Kitchen) {

    }
    this.sub = this.route.params.subscribe(params => {
      this.tab = params['tab'];
    });
    this.httpService
      .DoGet(environment.APP_API_URL + '/order/index')
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.IndexModel.ListDeliveryStatus = msg.Obj.ListDeliveryStatus;
          this.IndexModel.ListOrderType = msg.Obj.ListOrderType;
          this.IndexModel.ListSourceType = msg.Obj.ListSourceType;
          this.IndexModel.ListOrderCountModel = msg.Obj.ListOrderCountModel;
          this.getCount();
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });
    this.getDataFromParam(this.tab);
  }

  ngOnInit() {
    //this.activeTab();
    this.searchOrders(1);
  }
  getCountModel() {
    this.httpService
      .DoGet(environment.APP_API_URL + '/order/getcount')
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {          
          this.IndexModel.ListOrderCountModel = msg.Obj;
          this.getCount();
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });
  }
  getCount() {
    var CountTab = this.IndexModel.ListOrderCountModel.find(x => x.Status == -1  && x.RequestType == -1);
    var CountTab1 = this.IndexModel.ListOrderCountModel.find(x => x.Status == 0 && x.RequestType == -1);
    var CountTab2 = this.IndexModel.ListOrderCountModel.find(x => x.Status == 1 && x.RequestType == -1);
    var CountTab3 = this.IndexModel.ListOrderCountModel.find(x => x.Status == 2 && x.RequestType == -1);
    var CountTab4 = this.IndexModel.ListOrderCountModel.find(x => x.Status == 3 && x.RequestType == -1);
    var CountTab5 = this.IndexModel.ListOrderCountModel.find(x => x.Status == 4 && x.RequestType == -1);
    var CountTab6 = this.IndexModel.ListOrderCountModel.find(x => x.Status == 5 && x.RequestType == -1);
    var CountTab7 = this.IndexModel.ListOrderCountModel.find(x => x.Status == 6 && x.RequestType == -1);
    var CountTab8 = this.IndexModel.ListOrderCountModel.find(x => x.Status == 7 && x.RequestType == -1);
    var CountTab9 = this.IndexModel.ListOrderCountModel.find(x => x.Status == -1 && x.RequestType == 1);
    var CountTab10 = this.IndexModel.ListOrderCountModel.find(x => x.Status == -1 && x.RequestType == 3);
    var CountTab11 = this.IndexModel.ListOrderCountModel.find(x => x.Status == 8 && x.RequestType == -1);

    this.Count.CountTab = CountTab !== undefined? CountTab.Count : 0;
    this.Count.CountTab1 = CountTab1 !== undefined? CountTab1.Count : 0;
    this.Count.CountTab2 = CountTab2 !== undefined? CountTab2.Count : 0;
    this.Count.CountTab3 = CountTab3 !== undefined? CountTab3.Count : 0;
    this.Count.CountTab4 = CountTab4 !== undefined? CountTab4.Count : 0;
    this.Count.CountTab5 = CountTab5 !== undefined? CountTab5.Count : 0;
    this.Count.CountTab6 = CountTab6 !== undefined? CountTab6.Count : 0;
    this.Count.CountTab7 = CountTab7 !== undefined? CountTab7.Count : 0;
    this.Count.CountTab8 = CountTab8 !== undefined? CountTab8.Count : 0;
    this.Count.CountTab9 = CountTab9 !== undefined? CountTab9.Count : 0;
    this.Count.CountTab10 = CountTab10 !== undefined? CountTab10.Count : 0;
    this.Count.CountTab11 = CountTab11 !== undefined? CountTab11.Count : 0;

  }
  getDataFromParam(tab) {
    switch (tab) {
      case "tab":
        this.IndexModel.Status = -1;
        this.IndexModel.RequestType = -1;
        break;
      case "tab1":
        this.IndexModel.Status = 0;
        this.IndexModel.RequestType = -1;

        break;
      case "tab2":
        this.IndexModel.Status = 1;
        this.IndexModel.RequestType = -1;
        break;
      case "tab3":
        this.IndexModel.Status = 2;
        this.IndexModel.RequestType = -1;
        break;
      case "tab4":
        this.IndexModel.Status = 3;
        this.IndexModel.RequestType = -1;
        break;
      case "tab5":
        this.IndexModel.Status = 4;
        this.IndexModel.RequestType = -1;
        break;
      case "tab6":
        this.IndexModel.Status = 5;
        this.IndexModel.RequestType = -1;
        break;
      case "tab7":
        this.IndexModel.Status = 6;
        this.IndexModel.RequestType = -1;
        break;
      case "tab8":
        this.IndexModel.Status = 7;
        this.IndexModel.RequestType = -1;
        break;
      case "tab9":
        this.IndexModel.Status = -1;
        this.IndexModel.RequestType = 1;
        break;
      case "tab10":
        this.IndexModel.Status = -1;
        this.IndexModel.RequestType = 3;
        break;
      case "tab11":
        this.IndexModel.Status = 8;
        this.IndexModel.RequestType = -1;
        break;
    }
  }

  onSelectTab(data: TabDirective, tab) {
    this.router.navigate(['/order/list/' + tab],
      {
        relativeTo: this.route
      });
    this.getDataFromParam(tab);
    this.searchOrders(1);
    //this.orderlist.searchOrders();

  }
  pageChanges(event) {
    this.SearchOrder.PageIndex = event;
    this.searchOrders(event);
  }
  updateSearchModel(data: OrderSearchModel) {
    this.SearchOrder = data;
    this.searchOrders(1);
  }
  searchOrders(page) {
    this.IsShowLoader = !this.IsShowLoader;
    this.ListData = [];
    if (this.SearchOrder.ArrCreatedDate.length > 0) {
      this.SearchOrder.FromCreatedDate = this.SearchOrder.ArrCreatedDate[0];
      this.SearchOrder.ToCreatedDate = this.SearchOrder.ArrCreatedDate[1];
    }
    this.SearchOrder.Status = this.IndexModel.Status;
    this.SearchOrder.RequestType = this.IndexModel.RequestType;
    this.SearchOrder.KeyWord = this.SearchOrder.KeyWord != null ? this.SearchOrder.KeyWord.trim() : '';
    this.SearchOrder.PageIndex = page;
    this.httpService
      .DoPost(environment.APP_API_URL + '/order/search', this.SearchOrder)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.ListData = msg.Obj.Data;
          this.page = msg.Obj.Pager;
          this.SearchOrder.PageIndex = msg.Obj.Pager.CurrentPage;
          this.SearchOrder.PageSize = msg.Obj.Pager.PageSize;
        } else {
          this.ListData = [];
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
        this.IsShowLoader = !this.IsShowLoader;
      });
  }

}
