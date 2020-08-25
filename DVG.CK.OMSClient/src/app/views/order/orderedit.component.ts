import { Component, OnInit, TemplateRef, ViewChild, ComponentRef } from '@angular/core';
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
import { OrderIndexModel, OrderSearchModel, OrderFullViewModel, OrderDetailViewModel } from '../../model/OrderModel';
import { Pager } from '../../model/ResponseData';
import { ActivatedRoute, Router } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { OrderStatusEnum } from '../../utils/enums/order.enum';
import { OrderListComponent } from './orderlist.component';
import { RolesEnum } from '../../utils/enums/roles.enum';
import { AuthenService } from '../../services/authen.service';
import {
  DateTimeAdapter,
  OWL_DATE_TIME_FORMATS,
  OWL_DATE_TIME_LOCALE
} from 'ng-pick-datetime';
import { MomentDateTimeAdapter } from 'ng-pick-datetime-moment';
import { Page } from 'ngx-pagination/dist/pagination-controls.directive';
import { FormControl } from '@angular/forms';

export const FORMATS_DATE = {
  parseInput: 'DD/MM/YYYY HH:mm',
  fullPickerInput: 'DD/MM/YYYY HH:mm',
  datePickerInput: 'DD/MM/YYYY HH:mm',
  timePickerInput: 'LT',
  monthYearLabel: 'MMM YYYY',
  dateA11yLabel: 'DD/MM/YYYY',
  monthYearA11yLabel: ' YYYY MMMM'
};

@Component({
  selector: 'app-orderedit',
  templateUrl: './orderedit.component.html',
  styleUrls: ['./orderedit.component.css'],
  providers: [
    {
      provide: DateTimeAdapter,
      useClass: MomentDateTimeAdapter,
      deps: [OWL_DATE_TIME_LOCALE]
    },
    { provide: OWL_DATE_TIME_FORMATS, useValue: FORMATS_DATE }
  ]
})
export class OrderEditComponent implements OnInit {
  Order = new OrderFullViewModel();
  public sub: any;
  IsShowLoader = false;
  RolesEnum = RolesEnum;
  Role: any;
  encryptId = "";
  SearchProductModel = {
    KeyWord: '',
    PageIndex: 1,
    PageSize: 10,
  };
  OrderStatusEnum = OrderStatusEnum;
  Error = {
    OrderTpye: false,
    DeliveryDate: false,
    FormatDeliveryDate: false,
    SourceType: false,
    CustomerName: false,
    CustomerPhone: false,
    FormatCustomerPhone: false,
    District: false,
    DeliveryAddress: false,
    ListProduct: false
  }
  wardName: string;
  districtName: string;
  page = new Pager();

  isActiveTab1: boolean = true;
  isActiveTab2: boolean = false;
  isActiveTab3: boolean = false;

  @ViewChild('gmap') gmapElement: any;
  gmap: google.maps.Map;
  gmapMarker: google.maps.Marker;
  @ViewChild('orderMapModel') public orderMapModel: ModalDirective;
  keyword = "SG";
  lstAddressSuggest: any[] = [];
  lstCustomers: any[] = [];
  searchPhone = '';
  myControl = new FormControl();
  options: string[] = ['One', 'Two', 'Three'];
  constructor(
    private modalService: BsModalService,
    private httpService: HttpService,
    private notifier: NotifierService,
    private titleService: Title,
    private route: ActivatedRoute,
    private router: Router
  ) {

    this.sub = this.route.queryParams.subscribe(params => {
      this.encryptId = params.encryptId;
    });
    if (this.encryptId != "" && this.encryptId != null)
      this.titleService.setTitle(`Sửa đơn hàng - ${environment.APP_TITLE}`);
    else
      this.titleService.setTitle(`Tạo đơn hàng - ${environment.APP_TITLE}`);

    
    var param = {
      encryptId: this.encryptId
    }
    this.httpService
      .DoGet(environment.APP_API_URL + '/order/update', param)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.Order = msg.Obj;
          //this.Order.SourceType = this.Order.SourceType > -1 ? this.Order.SourceType : null;
          //this.Order.OrderType = this.Order.OrderType > -1 ? this.Order.OrderType : null;
          //this.Order.DistrictId = this.Order.DistrictId > 0 ? this.Order.DistrictId : null;
          //this.Order.WardId = this.Order.WardId > 0 ? this.Order.WardId : null;
          //var discount = this.Order.Discount / 100 * this.Order.OrderPrice;
          //this.Order.DiscountStr = "0";
          this.searchProduct(1);
          this.onChangeTab(this.Order.TabIndex);
            
        } else {
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });

    

  }

  ngOnInit() {
    
  }
  searchProduct(page) {
    this.SearchProductModel.KeyWord = this.SearchProductModel.KeyWord.trim();
    this.SearchProductModel.PageIndex = page;
    this.httpService
      .DoPost(environment.APP_API_URL + '/product/search', this.SearchProductModel, null)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.Order.lstOrderDetailViewModel = msg.Obj.Data;
          this.page = msg.Obj.Pager;
        } else {
          this.Order.lstOrderDetailViewModel = [];
          this.page = new Pager();
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });
  }
  pageChange(event) {
    this.SearchProductModel.PageIndex = event;
    this.searchProduct(event);
  }

  insertProduct(item) {
    var id = item.ProductId;
    var isExist = false;

    if (this.Order.ListProductViewModel.length > 0) {

      this.Order.ListProductViewModel.filter((x, i) => {
        if (x.ProductId == item.ProductId) {
          isExist = true;
          x.Quantity = +x.Quantity + 1;

          return;
        }
      });
      if (!isExist) {
        this.Order.ProductViewModelItem = new OrderDetailViewModel();
        this.Order.ProductViewModelItem.ProductId = id;
        this.Order.ProductViewModelItem.Quantity = 1;
        this.Order.ProductViewModelItem.OriginPrice = item.OriginPrice;
        this.Order.ProductViewModelItem.Price = item.Price;
        this.Order.ProductViewModelItem.PriceStr = this.FormatNumber(item.Price);
        this.Order.ProductViewModelItem.OriginPriceStr = this.FormatNumber(item.OriginPrice);
        this.Order.ProductViewModelItem.Note = '';
        this.Order.ProductViewModelItem.ProductName = item.ProductName;
        this.Order.ProductViewModelItem.Code = item.Code;
        this.Order.ListProductViewModel.push(this.Order.ProductViewModelItem);
      }
    }

    else {
      this.Order.ProductViewModelItem = new OrderDetailViewModel();
      this.Order.ProductViewModelItem.ProductId = id;
      this.Order.ProductViewModelItem.Quantity = 1;
      this.Order.ProductViewModelItem.OriginPrice = item.OriginPrice;
      this.Order.ProductViewModelItem.Price = item.Price;
      this.Order.ProductViewModelItem.PriceStr = item.PriceStr;
      this.Order.ProductViewModelItem.OriginPriceStr = item.OriginPriceStr;
      this.Order.ProductViewModelItem.Note = '';
      this.Order.ProductViewModelItem.ProductName = item.ProductName;
      this.Order.ProductViewModelItem.Code = item.Code;
      this.Order.ListProductViewModel.push(this.Order.ProductViewModelItem);
    }
    this.setPrice();
  }
  onKeyPressQuanity(event) {
    var regex = /^[0-9]\d*$/;
    //const e = <KeyboardEvent>event;
    //console.log("event", event);
    if (!regex.test(event.key)) {
      event.preventDefault();
      return false;
    }
  }
  onChangeQuanity(item) {
    if (item.Quantity < 1) {
      item.Quantity = 1;
      this.notifier.notify(NotifierTypeConst.Warning, "Số lượng cần lớn hơn hoặc bằng 1 !");
    }
    this.setPrice();
  }
  decreaseProduct(item) {
    this.Order.ListProductViewModel = this.Order.ListProductViewModel.filter(x => x.ProductId !== item.ProductId);
    this.setPrice();
  }

  setPrice() {
    var price;
    var totalprice = 0;
    var discount = 0;
    for (var i = 0; i < this.Order.ListProductViewModel.length; i++) {
      price = this.Order.ListProductViewModel[i].Price * this.Order.ListProductViewModel[i].Quantity
      totalprice += price;
    }
    this.Order.OrderOriginPrice = totalprice;
    this.Order.OrderOriginPriceStr = this.FormatNumber(totalprice);
    discount = this.Order.Discount / 100 * totalprice;
    this.Order.DiscountStr = this.FormatNumber(discount);
    this.Order.OrderPrice = this.Order.OrderOriginPrice - discount + this.Order.ShipFee;
    this.Order.OrderPriceStr = this.FormatNumber(totalprice - discount + this.Order.ShipFee);

  }

  delItem(item) {
    var index = this.Order.ListProductViewModel.findIndex(x => x.ProductId == item.ProductId);
    if (index > -1)
      this.Order.ListProductViewModel[index].Quantity = +this.Order.ListProductViewModel[index].Quantity - 1;

    this.setPrice();
  }
  addItem(item) {
    var index = this.Order.ListProductViewModel.findIndex(x => x.ProductId == item.ProductId);
    if (index > -1)
      this.Order.ListProductViewModel[index].Quantity = +this.Order.ListProductViewModel[index].Quantity + 1;
    this.setPrice();
  }


  FormatNumber(sNumber, sperator = ".") {
    var result = sNumber + "";
    result = result.replace(/(.)(?=(\d{3})+$)/g, '$1.');
    return result;

  }

  onChangeDistrict() {
    this.Order.WardId = null;
    this.getShipByDistrict();
    this.initWardData(this.Order.DistrictId);
    this.Order.DeliveryAddress = '';
    if (this.Order.DistrictId > 0) {
      this.Order.ListDistrict.map((item, i) => {
        if (item.district_id == this.Order.DistrictId) {
          this.districtName = item.district_name;
          if (this.Order.WardId > 0)
            this.Order.DeliveryAddress = "Phường " + this.wardName + ", " + item.district_name;
          else
            this.Order.DeliveryAddress = item.district_name;
        }
      })

    }
    this.validate();
  }

  onChangeWard() {
    this.Order.DeliveryAddress = '';
    if (this.Order.WardId > 0) {
      this.Order.ListWard.map((item, i) => {
        if (item.ward_id == this.Order.WardId) {
          this.wardName = item.ward_name;
          this.Order.DeliveryAddress = "Phường " + item.ward_name + ", " + this.districtName;
        }
      });
    } else if (this.Order.DistrictId > 0) {
      this.Order.DeliveryAddress = this.districtName;
    }
  }
  getShipByDistrict() {
    if (this.Order.OrderType == 0 && this.Order.DistrictId > 0) {
      this.Order.ListDistrict.map((item, i) => {
        if (item.district_id == this.Order.DistrictId) {
          this.Order.ShipFee = item.ship_fee;
          this.Order.ShipFeeStr = this.FormatNumber(item.ship_fee);
        }
      })
    } else {
      this.Order.ShipFee = 0;
      this.Order.ShipFeeStr = "0";
    }
    this.setPrice();
  }

  initWardData(district_id) {
    var param = {
      districtId: district_id
    };
    this.httpService
      .DoPost(environment.APP_API_URL + '/ward/searchbydistrictid', null, param)
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.Order.ListWard = msg.Obj;
        } else {
          this.Order.ListWard = [];
          this.notifier.notify(NotifierTypeConst.Error, msg.Title);
        }
      });
  }

  doUpdate(status) {
    if (!this.validate()) {
      this.showErrNotify();
      return;
    }
    else {
      this.Order.Status = status;
      this.Order.DistrictId = this.Order.DistrictId == null ? 0 : this.Order.DistrictId;
      this.Order.WardId = this.Order.WardId == null ? 0 : this.Order.WardId;

      this.httpService
        .DoPost(environment.APP_API_URL + '/order/updateorder', this.Order, null)
        .subscribe(msg => {
          if (msg != null && msg.Error === false) {
            if (this.Order.OrderId == 0)
              this.notifier.notify(NotifierTypeConst.Success, "Thêm đơn hàng thành công !");
            else
              this.notifier.notify(NotifierTypeConst.Success, "Cập nhật đơn hàng thành công !");
            window.setTimeout(function () {
              if (status == 0) {
                location.href = '/order/list/tab1';
              } else if (status == 1) {
                location.href = '/order/list/tab2';
              }

            }, 200);
          } else {
            this.notifier.notify(NotifierTypeConst.Error, msg.Title);
          }
        });
    }

  }

  validate() {
    if (this.Order.OrderType == null || this.Order.OrderType < 0) {
      this.Error.OrderTpye = true;
      return false;
    } else {
      this.Error.OrderTpye = false;
    }
    if (this.Order.DeliverDate == null || this.Order.DeliverDate == '') {
      this.Error.DeliveryDate = true;
      return false;
    } else {
      this.Error.DeliveryDate = false;
      //var regex = /^(0?[1-9]|[12][0-9]|3[01])\/(0?[1-9]|1[0-2])\/[0-9]{4} (00|[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9])$/;
      //if (this.Order.DeliverDate == null || this.Order.DeliverDate == '') {
      //  this.Error.FormatDeliveryDate = true;
      //} else {
      //  if (!regex.test(this.Order.DeliverDate)) {
      //    this.Error.FormatDeliveryDate = true;
      //    if (showErr)
      //      this.notifier.notify(NotifierTypeConst.Error, "Ngày giao hàng không đúng định dạng !");
      //    return false;
      //  } else {
      //    this.Error.FormatDeliveryDate = false;
      //  }
      //}
    }
    if (this.Order.SourceType == null || this.Order.SourceType < 0) {
      this.Error.SourceType = true;
      return false;
    } else {
      this.Error.SourceType = false;
    }
    if (this.Order.CustomerName == null || this.Order.CustomerName.trim() == '') {
      this.Error.CustomerName = true;
      return false;
    } else {
      this.Error.CustomerName = false;
    }
    if (this.Order.CustomerPhone == null || this.Order.CustomerPhone.trim() == '') {
      this.Error.CustomerPhone = true;
      return false;
    } else {
      this.Error.CustomerPhone = false;
      var regexMobile1 = /^0[1-9][0-9]{8,9}$/;
      var regexMobile2 = /^84[0-9]{8,9}$/;
      if (!regexMobile1.test(this.Order.CustomerPhone) && !regexMobile2.test(this.Order.CustomerPhone)) {
        this.Error.FormatCustomerPhone = true;
        return false;
      } else {
        this.Error.FormatCustomerPhone = false;
      }
    }
    //if (this.Order.OrderType == 0) {
    //  if (this.Order.DistrictId == null || this.Order.DistrictId < 0) {
    //    this.Error.District = true;
    //    return false;
    //  } else {
    //    this.Error.District = false;
    //  }
    //}
    if (this.Order.OrderType == 0) {
      if (this.Order.DeliveryAddress == null || this.Order.DeliveryAddress.trim() == '') {
        this.Error.DeliveryAddress = true;
        return false;
      } else {
        this.Error.DeliveryAddress = false;
      }
    }
    if (this.Order.ListProductViewModel == null || this.Order.ListProductViewModel.length == 0) {
      this.Error.ListProduct = true;
      return false;
    } else {
      this.Error.ListProduct = false;
    }

    return true;
  }

  showErrNotify() {
    //if (this.Error.OrderTpye) {
    //  this.notifier.notify(NotifierTypeConst.Error, "Vui lòng chọn hình thức giao hàng !");
    //  return;
    //}
    if (this.Error.CustomerPhone) {
      this.notifier.notify(NotifierTypeConst.Error, "Vui lòng nhập SĐT khách hàng !");
      return;
    }
    if (this.Error.FormatCustomerPhone) {
      this.notifier.notify(NotifierTypeConst.Error, "SĐT không hợp lệ !");
      return;
    }
    if (this.Error.CustomerName) {
      this.notifier.notify(NotifierTypeConst.Error, "Vui lòng nhập tên khách hàng !");
      return;
    }    
    if (this.Error.DeliveryDate) {
      this.notifier.notify(NotifierTypeConst.Error, "Vui lòng chọn thời gian giao hàng !");
      return;
    }
    if (this.Error.DeliveryAddress) {
      this.notifier.notify(NotifierTypeConst.Error, "Vui lòng nhập địa chỉ giao hàng !");
      return;

    }
    if (this.Error.SourceType) {
      this.notifier.notify(NotifierTypeConst.Error, "Vui lòng chọn nguồn đơn hàng !");
      return;
    }   

    //if (this.Error.District) {
    //  this.notifier.notify(NotifierTypeConst.Error, "Vui lòng chọn quận/huyện !");
    //  return;
    //}
    
    if (this.Error.ListProduct) {
      this.Error.ListProduct = true;
      this.notifier.notify(NotifierTypeConst.Error, "Vui lòng thêm sản phẩm vào giỏ hàng !");
      return;
    }
  }

  onChangeOrderType() {
    if (this.Order.OrderType == 0) {
      this.getShipByDistrict();
    } else {
      this.Order.ShipFee = 0;
      this.Order.ShipFeeStr = '0';
    }
    this.validate();
    this.setPrice();
  }

  onChangeTab(id) {
    switch (id) {
      case 1:
        this.isActiveTab1 = true;
        this.isActiveTab2 = false;
        this.isActiveTab3 = false;
        this.Order.OrderType = 0;
        break;
      case 2:
        this.isActiveTab2 = true;
        this.isActiveTab3 = false;
        this.isActiveTab1 = false;
        this.Order.OrderType = 0;
        break;
      case 3:
        this.isActiveTab3 = true;
        this.isActiveTab1 = false;
        this.isActiveTab2 = false;
        this.Order.OrderType = 1;
        break;
    }
  }

  public InitGmap(address) {
    new google.maps.Geocoder().geocode({ address: address }, (response, status) => {
      let lat = 0;
      let lng = 0;

      if (status === google.maps.GeocoderStatus.OK) {
        lat = response[0].geometry.location.lat();
        lng = response[0].geometry.location.lng();
      }

      const mapProp = {
        center: new google.maps.LatLng(lat, lng),
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP
      };

      this.gmap = new google.maps.Map(this.gmapElement.nativeElement, mapProp);
      this.gmapMarker = new google.maps.Marker({
        position: new google.maps.LatLng(lat, lng),
        map: this.gmap
      });
    });
    google.maps.event.addListener(this.gmap, 'click', (event) => {
      this.placeMarker(event);
    });
  }
  placeMarker(event) {

    var marker = new google.maps.Marker({
      position: event.latLng,
      map: this.gmap
    });
    console.log(event.latLng.lat() + " " + event.latLng.lng());
  }
  public showMap() {
    this.orderMapModel.show();
    this.InitGmap((this.Order.DeliveryAddress != '' && this.Order.DeliveryAddress != null) ? this.Order.DeliveryAddress : 'Sài Gòn');
  }

  getSuggestAddress(event: any) {
    var text = event.target.value.trim();
    if (text.length > 3) {
      var param = {
        keyword: event.target.value
      }
      this.httpService
        .DoPost(environment.APP_API_URL + '/order/getsuggestaddress', null, param)
        .subscribe(msg => {
          if (msg != null && msg.Error === false) {
            this.lstAddressSuggest = msg.Obj;
          } else {
            this.notifier.notify(NotifierTypeConst.Error, msg.Title);
          }
        });
    }
  }
  onClearAddress() {
    this.lstAddressSuggest = [];
  }
  selectedAddress(event) {
    var selectedItem = this.lstAddressSuggest.find(x => x.FormattedAddress == event.option.value);
    if (selectedItem != null) {
      const mapProp = {
        center: new google.maps.LatLng(selectedItem.Lat, selectedItem.Lng),
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP
      };

      this.gmap = new google.maps.Map(this.gmapElement.nativeElement, mapProp);
      this.gmapMarker = new google.maps.Marker({
        position: new google.maps.LatLng(selectedItem.Lat, selectedItem.Lng),
        map: this.gmap
      });
    }
  }
  searchCustomerByPhone(keyword: any) {
    this.lstCustomers = [];
    var text = keyword.target.value.trim();
    if (text.length > 3) {
      var param = {
        keyword: keyword.target.value
      }
      this.httpService
        .DoPost(environment.APP_API_URL + '/order/searchcustomerbyphone', null, param)
        .subscribe(msg => {
          if (msg != null && msg.Error === false) {
            this.lstCustomers = msg.Obj;
          } else {
            this.notifier.notify(NotifierTypeConst.Error, msg.Title);
          }
        });
    }
    
  }

  selectedPhone(phone) {
    var selectedItem = this.lstCustomers.find(x => x.Phone == phone.option.value);
    if (selectedItem != null) {
      this.Order.CustomerName = selectedItem.Name;
      this.Order.DeliveryAddress = selectedItem.Address;
      this.Order.DistrictId = selectedItem.DistrictId;
      this.lstCustomers = [];
    }
  }
}
