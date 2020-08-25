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
import { OrderIndexModel, OrderSearchModel, OrderFullViewModel } from '../../model/OrderModel';
import { Pager } from '../../model/ResponseData';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { OrderStatusEnum, DeliveryStatusEnum, RequestTypeEnum } from '../../utils/enums/order.enum';
import { OrderListComponent } from './orderlist.component';
import { RolesEnum } from '../../utils/enums/roles.enum';
import { AuthenService } from '../../services/authen.service';

@Component({
  selector: 'app-orderdetail',
  templateUrl: './orderdetail.component.html',
  styleUrls: ['./orderdetail.component.css']
})
export class OrderDetailComponent implements OnInit {
  OrderDetail = new OrderFullViewModel();
  RolesEnum = RolesEnum;
  DeliveryStatusEnum = DeliveryStatusEnum;
  RequestTypeEnum = RequestTypeEnum;
  OrderStatusEnum = OrderStatusEnum;
  Role: any;
  constructor(
    private modalService: BsModalService,
    private httpService: HttpService,
    private notifier: NotifierService,
    private titleService: Title,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.Role = +AuthenService.getRoles();
  }

  ngOnInit() {      
  }

  backtolist() {

  }
 
}
