import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { LocationStrategy, HashLocationStrategy, PathLocationStrategy } from '@angular/common';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

import { AppComponent } from './app.component';

import { DefaultLayoutComponent } from './containers';

import { P404Component } from './views/error/404.component';
import { P405Component } from './views/error/405.component';
import { P500Component } from './views/error/500.component';
import { LoginComponent } from './views/login/login.component';
import { TreeModule } from 'angular-tree-component';
import { NotifierModule, NotifierOptions } from 'angular-notifier';
import { NgxNotifierModule } from 'ngx-notifier';



const APP_CONTAINERS = [
  DefaultLayoutComponent
];

/**
 * Custom angular notifier options
 */
const customNotifierOptions: NotifierOptions = {
  position: {
    horizontal: {
      position: 'right',
      distance: 12
    },
    vertical: {
      position: 'top',
      gap: 10
    }
  },
  behaviour: {
    autoHide: 3000,
    onClick: 'hide',
    onMouseover: 'pauseAutoHide',
    showDismissButton: true,
    stacking: 4
  },
  animations: {
    enabled: true,
    show: {
      preset: 'slide',
      speed: 300,
      easing: 'ease'
    },
    hide: {
      preset: 'fade',
      speed: 300,
      easing: 'ease',
      offset: 50
    },
    shift: {
      speed: 300,
      easing: 'ease'
    },
    overlap: 150
  }
};

import {
  AppAsideModule,
  AppBreadcrumbModule,
  AppHeaderModule,
  AppFooterModule,
  AppSidebarModule,
} from '@coreui/angular';

// Import routing module
import { AppRoutingModule } from './app.routing';

// Import 3rd party components
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ChartsModule } from 'ng2-charts/ng2-charts';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
// import { CKEditorModule } from 'ng2-ckeditor';
import { HomeComponent } from './views/home/home.component';
import { FilemanagerComponent } from './plugins/filemanager/filemanager.component';
import { LoaderComponent } from './plugins/loader/loader.component';
import { NgxLoadingModule } from 'ngx-loading';
import { NgSelectModule } from '@ng-select/ng-select';
import { OwlDateTimeModule, OwlNativeDateTimeModule } from 'ng-pick-datetime';
import { CrystalGalleryModule } from 'ngx-crystal-gallery';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxPaginationModule } from 'ngx-pagination';
import { ModalModule } from 'ngx-bootstrap/modal';
import { CarouselModule } from 'ngx-bootstrap/carousel';
// Collapse module
import { CollapseModule } from 'ngx-bootstrap/collapse';
// Tooltip module
// import { ArticlesComponent } from './views/articles/articles.component';

import { AccordionModule } from 'ngx-bootstrap/accordion';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { MatStepperModule } from '@angular/material/stepper';
import { LogonSSOComponent } from './views/login/logonsso.component';
import { NotificationComponent } from './views/notification/notification.component';
import { AddNotificationComponent } from './views/notification/addnotification.component';
import { OrderListComponent } from './views/order/orderlist.component';
import { OrderTabComponent } from './views/order/ordertab.component';
import { OrderEditComponent } from './views/order/orderedit.component';
import { OrderDetailComponent } from './views/order/orderdetail.component';
import { CashierRevenueStatisticComponent } from './views/statistic/cashierrevenuestatistic.component';
import { UsersComponent } from './views/users/user.component';
import { AccountComponent } from './views/Account/account.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule, MatTooltipModule } from '@angular/material';

@NgModule({
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule,
    AppAsideModule,
    AppBreadcrumbModule.forRoot(),
    AppFooterModule,
    AppHeaderModule,
    AppSidebarModule,
    PerfectScrollbarModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    ChartsModule,
    // tslint:disable-next-line: deprecation
    HttpClientModule,
    FormsModule,
    // CKEditorModule,
    TreeModule.forRoot(),
    HttpClientModule,
    NotifierModule.withConfig(customNotifierOptions),
    NgxLoadingModule.forRoot({}),
    NgSelectModule,
    NgxPaginationModule,
    ModalModule.forRoot(),
    CollapseModule.forRoot(),
    AccordionModule.forRoot(),
    TooltipModule.forRoot(),
    OwlDateTimeModule, // ng-datetime-picker
    OwlNativeDateTimeModule, // ng-datetime-picker
    MatStepperModule,
    CarouselModule.forRoot(),
    CrystalGalleryModule,
    NgxNotifierModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatTooltipModule
  ],
  declarations: [
    AppComponent,
    P404Component,
    P405Component,
    P500Component,
    LoginComponent,
    LogonSSOComponent,
    HomeComponent,
    FilemanagerComponent,
    LoaderComponent,
    NotificationComponent,
    AddNotificationComponent,
    DefaultLayoutComponent,
    OrderListComponent,
    OrderTabComponent,
    OrderEditComponent,
    OrderDetailComponent,
    CashierRevenueStatisticComponent,
    UsersComponent,
    AccountComponent
  ],
  providers: [{
    provide: LocationStrategy,
    useClass: PathLocationStrategy,
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
