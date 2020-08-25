import * as $ from 'jquery';
import { Component, OnDestroy, Inject, ViewEncapsulation, OnInit, AfterViewInit, AfterViewChecked, TemplateRef } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { navItems, NavData } from '../../_nav';
import { AuthenService } from '../../services/authen.service';
import { RolesEnum } from '../../utils/enums/roles.enum';
import { HttpService } from '../../services/http.service';
import { environment } from '../../../environments/environment';
import { NotifierTypeConst } from '../../utils/enums/notifier.enum';
import { NotifierService, NotifierModule } from 'angular-notifier';
import { NgxNotifierService } from 'ngx-notifier';
import { debug } from 'util';
import { style } from '@angular/animations';
import {
  BsModalService, BsModalRef,
} from 'ngx-bootstrap/modal';


@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html',
  styles: ['.sidebar .nav-link {font-size: 13px;}'],
  encapsulation: ViewEncapsulation.None,
})
export class DefaultLayoutComponent implements OnInit, AfterViewInit, AfterViewChecked, OnDestroy {
  private allowFixMenu = true;
  public IsShowSystemMenu = false;
  public UserName = '';
  public UserId: number;
  public navItems = navItems;
  public sidebarMinimized = true;
  private changes: MutationObserver;
  public element: HTMLElement;
  modalRef: BsModalRef;
  public IsShowNotifyAccount = false;
  private ModalType = '';
  public NotifyAccountCount = 0;

  constructor(private authenService: AuthenService,
    private httpService: HttpService,
    private notifier: NotifierService,
    private ngxNotifierService: NgxNotifierService,
    private modalService: BsModalService,
    @Inject(DOCUMENT) _document?: any) {
    this.changes = new MutationObserver((mutations) => {
      this.sidebarMinimized = _document.body.classList.contains('sidebar-minimized');
    });
    this.element = _document.body;
    this.changes.observe(<Element>this.element, {
      attributes: true,
      attributeFilter: ['class']
    });

    if (!AuthenService.IsAuthenticated) {
      this.httpService.doRedirectLogin();
      return;
    }

    this.UserName = AuthenService.getDisplayName();
    this.UserId = AuthenService.getUserId();
  }

  private leftMenuPermission(roleArr) {
    for (let i = 0; i < this.navItems.length; i++) {
      const children = this.navItems[i];
      let isPermission1 = true;
      if (children.roles != null) {
        isPermission1 = children.roles.some(x => roleArr.includes(x + ''));
      }
      if (!isPermission1) {
        children.attributes = { 'class': 'd-none' };
      } else {
        children.attributes = null;
      }

    }


  }

  public doLogout() {
    this.authenService.doLogout().subscribe();
  }

  ngOnInit(): void {
    // Roles
    let roles = AuthenService.getRoles();
    if (roles.length > 0) {
      this.IsShowNotifyAccount = roles === RolesEnum.CustomerService.toString() || roles === RolesEnum.KitchenManager.toString();
      const roleArr = roles.split(',');
      roles = ',' + roles + ',';
      this.IsShowSystemMenu = roles.includes(',' + RolesEnum.Admin + ',');
      // Phân quyền menu
      this.leftMenuPermission(roleArr);

    }
  }

  ngAfterViewInit(): void {
    setTimeout(() => { this.allowFixMenu = false; }, 1000);
    if (AuthenService.IsAuthenticated && this.IsShowNotifyAccount) {
      setInterval(() => {
        this.getNotifyAccount();
      }, 10000);

    }
  }

  ngAfterViewChecked(): void {
    if (this.allowFixMenu) {
      $('app-sidebar-nav-dropdown').each(function () {
        $(this).removeClass('open');
        $(this).find('app-sidebar-nav-items app-sidebar-nav-link a').each(function () {
          if ($(this).hasClass('active')) {
            const parent = $(this).closest('app-sidebar-nav-dropdown');
            if (!parent.hasClass('open')) {
              parent.addClass('open');
            }
          }
        });
      });
    }
  }

  private getNotifyAccount() {
    if (AuthenService.IsAuthenticated) {
      this.httpService
        .DoPost(environment.APP_API_URL + '/notify/getnotifyaccount')
        .subscribe(msg => {
          if (msg != null && msg.Error === false && msg.Obj.length > 0) {
            this.ngxNotifierService.clear();
            this.NotifyAccountCount = msg.Obj.length;
            // tslint:disable-next-line:max-line-length
            msg.Obj.forEach(item => {
              this.ngxNotifierService.createToast(item, 'danger');
            });
          }
        });
    }
  }

  public openModal(template: TemplateRef<any>, command) {
    this.ModalType = command;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public doConfirm() {
    switch (this.ModalType) {
      case 'clearAllNotifyAccount': {
        this.doClearAllNotifyAccount();
        break;
      }
    }
  }

  public doClearAllNotifyAccount() {
    this.httpService
      .DoPost(environment.APP_API_URL + '/notify/deletenotifyaccount')
      .subscribe(msg => {
        if (msg != null && msg.Error === false) {
          this.ngxNotifierService.clear();
          this.NotifyAccountCount = 0;
          this.modalRef.hide();
        }
      });
  }

  ngOnDestroy(): void {
    this.changes.disconnect();
  }


}
