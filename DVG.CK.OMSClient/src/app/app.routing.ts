import { CashierRevenueStatisticComponent } from './views/statistic/cashierrevenuestatistic.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Import Containers
import { DefaultLayoutComponent } from './containers';

import { P404Component } from './views/error/404.component';
import { P500Component } from './views/error/500.component';
import { P405Component } from './views/error/405.component';
import { LoginComponent } from './views/login/login.component';
import { FilemanagerComponent } from './plugins/filemanager/filemanager.component';
import { HomeComponent } from './views/home/home.component';
import { LogonSSOComponent } from './views/login/logonsso.component';
import { OrderTabComponent } from './views/order/ordertab.component';
import { OrderEditComponent } from './views/order/orderedit.component';
import { UsersComponent } from './views/users/user.component';
import { AccountComponent } from './views/Account/account.component';

export const routes: Routes = [
  // {
  //   path: '',
  //   redirectTo: 'home',
  //   pathMatch: 'full'
  // },
  {
    path: 'logonsso',
    component: LogonSSOComponent,
    data: {
      title: 'Logon SSO'
    }
  },
  {
    path: '404',
    component: P404Component,
    data: {
      title: 'Page 404'
    }
  },
  {
    path: '405',
    component: P405Component,
    data: {
      title: 'Page 405'
    }
  },
  {
    path: '500',
    component: P500Component,
    data: {
      title: 'Page 500'
    }
  },
  {
    path: '',
    component: DefaultLayoutComponent,
    data: {
      title: 'Trang chủ'
    },
    children: [
      {
        path: '',
        component: HomeComponent,
        data: {
          title: 'Trang chủ'
        }
      }
    ]
  },
  {
    path: 'login',
    component: LoginComponent,
    data: {
      title: 'Login Page'
    }
  },
  {
    path: 'filemanager',
    component: FilemanagerComponent
  },
  {
    path: '',
    component: DefaultLayoutComponent,
    data: {
      title: 'Trang chủ'
    },
    children: [
      {
        path: 'order/list/:tab',
        component: OrderTabComponent,
        data: {
          title: 'Danh sách đơn hàng'
        }
      },
      {
        path: 'order/update',
        component: OrderEditComponent,
        data: {
          title: ''
        }
      },
      {
        path: 'statistic/cashierrevenue',
        component: CashierRevenueStatisticComponent,
        data: {
          title: 'Thống kê doanh thu'
        }
      },
      {
        path: 'user/list',
        component: UsersComponent,
        data: {
          title: 'Quản lý tài khoản'
        }
      },
      {
        path: 'account/changepassword',
        component: AccountComponent,
        data: {
          title: 'Đổi mật khẩu'
        }
      }
    ]
  },
  { path: '**', component: P404Component }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
