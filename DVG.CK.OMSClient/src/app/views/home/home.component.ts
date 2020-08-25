import { Component, OnInit } from '@angular/core';
import { AuthenService } from '../../services/authen.service';
import { Title } from '@angular/platform-browser';
import { environment } from '../../../environments/environment';
import { HttpService } from '../../services/http.service';
import { GetListNoticeParam, NoticeModel } from '../../model/NoticeModel';
import { Observable } from 'rxjs';
import { NotifierService } from 'angular-notifier';
import { NotifierTypeConst } from '../../utils/enums/notifier.enum';
@Component({
  templateUrl: 'home.component.html'
})
export class HomeComponent implements OnInit {
  public noticeParam: GetListNoticeParam;
  public lstNotice: NoticeModel[];
  public  totalItems: number;
  public currentPage: number;
  public totalPage: number;
  public loading = false;
  public UserName = '';
  constructor(
    private notifier: NotifierService,
    private titleService: Title,
    private httpService: HttpService) {
    this.titleService.setTitle(`Trang chủ - ${environment.APP_TITLE}`);
    this.UserName = AuthenService.getDisplayName();

    // call check authorize server
    // this.loading = !this.loading;
    // this.httpService.DoGet(environment.APP_API_URL + '/home/get').subscribe();
    // notice param
    this.noticeParam = new GetListNoticeParam();
    this.noticeParam.Status = 1;
    this.noticeParam.PageIndex = 1;
  }
  ngOnInit(): void {
    // get list notice
   // his.doGetListNotice();
  }

  public getListNotice(): Observable<any> {
    this.loading = !this.loading;
    return this.httpService.DoPost(environment.APP_API_URL + '/notice/getnotices', this.noticeParam, null);
  }

  public doGetListNotice() {
    this.getListNotice().subscribe(msg => {
      if (msg != null && msg.Error === false) {
        this.lstNotice = <NoticeModel[]>msg.Obj.ListData;
        this.totalItems = msg.Obj.Pager.TotalItem;
        this.totalPage = msg.Obj.Pager.TotalPage;
        this.noticeParam.PageSize = msg.Obj.Pager.PageSize;
        this.loading = !this.loading;
      } else {
        this.lstNotice = null;
        this.loading = !this.loading;
        this.notifier.notify(NotifierTypeConst.Error, msg.Title);
      }
    });
  }
  public pageChanged(event) {
    this.noticeParam.PageIndex = event;
    this.doGetListNotice();
  }
}
