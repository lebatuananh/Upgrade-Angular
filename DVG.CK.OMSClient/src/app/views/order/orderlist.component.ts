import { Component, OnInit, TemplateRef, Input, Output, EventEmitter, OnChanges, ViewChild } from '@angular/core';
import { environment } from '../../../environments/environment';
import { NotifierTypeConst } from '../../utils/enums/notifier.enum';
import { HttpService } from '../../services/http.service';
import { NotifierService } from 'angular-notifier';
import { TagsGetListModel, TagsModel, TagsChangeStatusModel } from '../../model/TagsModel';
import {
  BsModalRef,
  BsModalService,

  ModalDirective
} from 'ngx-bootstrap/modal';
import { Title } from '@angular/platform-browser';
import { ngxLoadingAnimationTypes } from 'ngx-loading';
import { OrderIndexModel, OrderSearchModel } from '../../model/OrderModel';
import { Pager } from '../../model/ResponseData';
import { RolesEnum } from '../../utils/enums/roles.enum';
import { DeliveryStatusEnum, RequestTypeEnum, OrderStatusEnum, OrderTypeEnum } from '../../utils/enums/order.enum';
import {
  DateTimeAdapter,
  OWL_DATE_TIME_FORMATS,
  OWL_DATE_TIME_LOCALE
} from 'ng-pick-datetime';
import { MomentDateTimeAdapter } from 'ng-pick-datetime-moment';
import { AuthenService } from '../../services/authen.service';
import { OrderDetailComponent } from './orderdetail.component';

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
  selector: 'app-orderlist',
  templateUrl: './orderlist.component.html',
  styleUrls: ['./orderlist.component.css'],
  providers: [
    {
      provide: DateTimeAdapter,
      useClass: MomentDateTimeAdapter,
      deps: [OWL_DATE_TIME_LOCALE]
    },
    { provide: OWL_DATE_TIME_FORMATS, useValue: FORMATS_DATE }
  ]
})
export class OrderListComponent implements OnInit {
  modalRef: BsModalRef;
  LoadingTemplate: TemplateRef<any>;
  NgxLoadingAnimationTypes = ngxLoadingAnimationTypes;

  @Input() IsShowLoader: any;
  @Input() IndexModel: OrderIndexModel;
  @Input() SearchOrder: OrderSearchModel;
  @Input() ListData: any;
  @Input() page: Pager;
  Role: any;

  @Output() pageChanges = new EventEmitter();
  @Output() updateSearchModel = new EventEmitter();
  @Output() getCountModel = new EventEmitter();

  orderId: number;
  action: string;
  message: string;
  reasonNote: string;
  deliveryPrice: number;
  IsOpen = false;
  RolesEnum = RolesEnum;
  DeliveryStatusEnum = DeliveryStatusEnum;
  RequestTypeEnum = RequestTypeEnum;
  OrderStatusEnum = OrderStatusEnum;
  OrderTypeEnum = OrderTypeEnum;

  @ViewChild('orderdetail') orderdetail: OrderDetailComponent;
  @ViewChild('orderDetailModel') public orderDetailModel: ModalDirective;

  constructor(
    private modalService: BsModalService,
    private httpService: HttpService,
    private notifier: NotifierService,
    private titleService: Title,
  ) {
    this.titleService.setTitle(`Quản lý đơn hàng - ${environment.APP_TITLE}`);
    //this.IsShowLoader = !this.IsShowLoader;
    this.Role = +AuthenService.getRoles();

  }

  ngOnInit() {
    //this.searchOrders();
    //console.log("indexmodel", this.IndexModel)
  }

  pageChange(event) {
    this.pageChanges.emit(event);
  }


  onChangeIsOpen(event: boolean) {
    this.IsOpen = event;
  }

  doSearch() {
    this.updateSearchModel.emit(this.SearchOrder);
  }

  public openModalDescriptionOtherStatus(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, { class: 'modal-lg' });
  }

  public openModalConfirm(template: TemplateRef<any>, action: string, id: number, event) {
    this.modalRef = this.modalService.show(template, { class: 'modal-md' });
    this.orderId = id;
    this.action = action;

    switch (action) {
      case 'AproveOrder':
        this.message = 'Duyệt đơn';
        break;
      case 'KitchenAccept':
        this.message = 'Bếp nhận nấu';
        break;
      case 'KitchenDone':
        this.message = 'Đã nấu xong';
        break;
      case 'CallAhamoveShipping':
        this.message = 'Gọi Ahamove';
        break;
      case 'CallAnotherShipping':
        this.message = 'Gọi ship ngoài';
        this.deliveryPrice = null;
        break;
      case 'CashierReceive':
        this.message = 'Nhận tiền';
        break;
      case 'DeliveryToOrderComplete':
      case 'FailureToOrderComplete':
        this.message = 'Hoàn thành đơn';
        break;
      case 'RequestDestroy':
        this.message = 'CS báo hủy';
        this.reasonNote = '';
        break;
      case 'RequestConfirmCustomer':
        this.message = 'CS đã báo khách';
        break;
      case 'Destroy':
        this.message = 'Hủy đơn';
        this.reasonNote = '';
        break;
    }
  }
  public confirm(): void {
    this.modalRef.hide();

    switch (this.action) {
      case 'AproveOrder':
        this.doAproveOrder(this.orderId).subscribe(p => {
          if (!p.Error) {
            this.notifier.notify(NotifierTypeConst.Success, 'Duyệt đơn thành công');
            this.getCountModel.emit();
            this.doSearch();
          } else {
            this.notifier.notify(NotifierTypeConst.Error, p.Title);
          }
        });
        break;
      case 'KitchenAccept':
        this.doKitchenAccept(this.orderId).subscribe(p => {
          if (!p.Error) {
            this.doPrintKitchenApi(this.orderId).subscribe(() => {
              console.log('call máy in kitchen done');
            });
            this.notifier.notify(NotifierTypeConst.Success, 'Bếp đã nhận nấu');
            this.getCountModel.emit();
            this.doSearch();
          } else {
            this.notifier.notify(NotifierTypeConst.Error, p.Title);
          }
        });
        break;
      case 'KitchenDone':
        this.doKitchenDone(this.orderId).subscribe(p => {
          if (!p.Error) {
            this.notifier.notify(NotifierTypeConst.Success, 'Xác nhận đã nấu xong');
            this.doPrintCashierApi(this.orderId).subscribe(() => {
              console.log('call máy in cashier done');
            });
            this.getCountModel.emit();
            this.doSearch();
          } else {
            this.notifier.notify(NotifierTypeConst.Error, p.Title);
          }
        });
        break;
      case 'CallAhamoveShipping':
        this.doCallAhamoveShipping(this.orderId).subscribe(p => {
          if (!p.Error) {
            this.notifier.notify(NotifierTypeConst.Success, 'Xác nhận đã gọi Ahamove thành công');
            this.getCountModel.emit();
            this.doSearch();
          } else {
            this.notifier.notify(NotifierTypeConst.Error, p.Title);
          }
        });
        break;
      case 'CallAnotherShipping':
        this.doCallAnotherShipping(this.orderId, this.deliveryPrice).subscribe(p => {
          if (!p.Error) {
            this.notifier.notify(NotifierTypeConst.Success, 'Đã báo gọi ship ngoài');
            this.getCountModel.emit();
            this.doSearch();
          } else {
            this.notifier.notify(NotifierTypeConst.Error, p.Title);
          }
        });
        break;
      case 'CashierReceive':
        this.doCashierReceive(this.orderId).subscribe(p => {
          console.log(p);
          if (!p.Error) {
            this.notifier.notify(NotifierTypeConst.Success, 'Xác nhận đã nhận tiền thành công');
            this.getCountModel.emit();
            this.doSearch();
          } else {
            this.notifier.notify(NotifierTypeConst.Error, p.Title);
          }
        });
        break;
      case 'DeliveryToOrderComplete':
        this.doDeliveryToOrderComplete(this.orderId).subscribe(p => {
          console.log(p);
          if (!p.Error) {
            this.notifier.notify(NotifierTypeConst.Success, 'Xác nhận đơn đã hoàn thành');
            this.getCountModel.emit();
            this.doSearch();
          } else {
            this.notifier.notify(NotifierTypeConst.Error, p.Title);
          }
        });
        break;
      case 'FailureToOrderComplete':
        this.doFailureToOrderComplete(this.orderId).subscribe(p => {
          console.log(p);
          if (!p.Error) {
            this.notifier.notify(NotifierTypeConst.Success, 'Xác nhận đơn đã hoàn thành');
            this.getCountModel.emit();
            this.doSearch();
          } else {
            this.notifier.notify(NotifierTypeConst.Error, p.Title);
          }
        });
        break;
      case 'RequestDestroy':
        this.doRequestDestroy(this.orderId, this.reasonNote).subscribe(p => {
          if (!p.Error) {
            this.notifier.notify(NotifierTypeConst.Success, 'Đã gửi yêu cầu hủy đơn');
            this.getCountModel.emit();
            this.doSearch();
          } else {
            this.notifier.notify(NotifierTypeConst.Error, p.Title);
          }
        });
        break;
      case 'RequestConfirmCustomer':
        this.doRequestConfirmCustomer(this.orderId).subscribe(p => {
          if (!p.Error) {
            this.notifier.notify(NotifierTypeConst.Success, 'Xác nhận đã báo lại khách');
            this.getCountModel.emit();
            this.doSearch();
          } else {
            this.notifier.notify(NotifierTypeConst.Error, p.Title);
          }
        });
        break;
      case 'Destroy':
        this.doDestroy(this.orderId, this.reasonNote).subscribe(p => {
          if (!p.Error) {
            this.notifier.notify(NotifierTypeConst.Success, 'Hủy đơn thành công');
            this.getCountModel.emit();
            this.doSearch();
          } else {
            this.notifier.notify(NotifierTypeConst.Error, p.Title);
          }
        });
        break;
    }
  }
  public decline(): void {
    this.modalRef.hide();
  }

  doAproveOrder(orderId) {
    const params = {
      orderId: orderId
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/aproveorder',
      null,
      params
    );
  }

  doKitchenAccept(orderId) {
    const params = {
      orderId: orderId
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/kitchenaccept',
      null,
      params
    );
  }

  doPrintKitchenApi(orderId) {
    const params = {
      orderId: orderId,
      type: 1
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/callprinter',
      null,
      params
    );
  }

  doPrintCashierApi(orderId) {
    const params = {
      orderId: orderId,
      type: 2
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/callprinter',
      null,
      params
    );
  }

  doKitchenDone(orderId) {
    const params = {
      orderId: orderId
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/kitchendone',
      null,
      params
    );
  }
  doCallAhamoveShipping(orderId) {
    const params = {
      orderId: orderId
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/callahamoveshipping',
      null,
      params
    );
  }
  doCallAnotherShipping(orderId, deliveryPrice) {
    const params = {
      orderId: orderId,
      shipFee: deliveryPrice
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/callanothershipping',
      null,
      params
    );
  }
  doCashierReceive(orderId) {
    const params = {
      orderId: orderId
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/cashierreceive',
      null,
      params
    );
  }
  doDeliveryToOrderComplete(orderId) {
    const params = {
      orderId: orderId
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/deliverytoordercomplete',
      null,
      params
    );
  }
  doFailureToOrderComplete(orderId) {
    const params = {
      orderId: orderId
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/failuretoordercomplete',
      null,
      params
    );
  }
  doRequestDestroy(orderId, reasonNote) {
    const params = {
      orderId: orderId,
      reasonNote: reasonNote
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/requestdestroy',
      null,
      params
    );
  }
  doRequestConfirmCustomere(orderId) {
    const params = {
      orderId: orderId
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/requestconfirmcustomer',
      null,
      params
    );
  }
  doRequestConfirmCustomer(orderId) {
    const params = {
      orderId: orderId
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/requestconfirmcustomer',
      null,
      params
    );
  }
  doDestroy(orderId, reasonNote) {
    const params = {
      orderId: orderId,
      reasonNote: reasonNote
    };
    return this.httpService.DoPost(
      environment.APP_API_URL + '/order/destroy',
      null,
      params
    );
  }

  getOrderById(orderId) {
    const params = {
      orderId: orderId
    };
    this.httpService.DoPost(
      environment.APP_API_URL + '/order/getorderbyid',
      null,
      params
    ).subscribe(msg => {
      if (msg != null && msg.Error === false) {
        this.orderDetailModel.show();
        this.orderdetail.OrderDetail = msg.Obj;
      }
    })
  }
}
