import { RolesEnum } from './utils/enums/roles.enum';
import { environment } from '../environments/environment.staging';

interface NavAttributes {
  [propName: string]: any;
}
interface NavWrapper {
  attributes: NavAttributes;
  element: string;
}
interface NavBadge {
  text: string;
  variant: string;
}
interface NavLabel {
  class?: string;
  variant: string;
}

export interface NavData {
  name?: string;
  url?: string;
  icon?: string;
  badge?: NavBadge;
  title?: boolean;
  children?: NavData[];
  variant?: string;
  attributes?: NavAttributes;
  divider?: boolean;
  class?: string;
  label?: NavLabel;
  wrapper?: NavWrapper;
  roles?: number[];
  notroles?: number[];
  group?: number[];
  notgroup?: number[];
}
export const navItems: NavData[] = [
  //{
  //  name: 'Orders Management',
  //  icon: 'fa fa-users text-warning',
  //  class: 'text-warning',
  //  children: [
  //    {
  //      name: 'Customers',
  //      url: '/Users/Customers',
  //      icon: 'fa fa-user',
  //      roles: [RolesEnum.Admin, RolesEnum.MemberManagement]
  //    },
  //    {
  //      name: 'User crawler',
  //      url: '/Users/UserCrawler',
  //      icon: 'fa fa-user-secret',
  //      roles: [RolesEnum.Admin, RolesEnum.UserCrawlerManagement]
  //    },
  //  ]
  //},
  {
    name: 'Tạo đơn hàng',
    url: '/order/update',
    icon: 'fa fa-plus',
    roles: [RolesEnum.Admin, RolesEnum.CustomerService]
  },
  {
    name: 'Danh sách đơn hàng',
    url: '/order/list/tab1',
    icon: 'fa fa-list',
    roles: [RolesEnum.Admin, RolesEnum.CustomerService],
  },
  {
    name: 'Danh sách đơn hàng',
    url: '/order/list/tab3',
    icon: 'fa fa-list',
    roles: [RolesEnum.Kitchen, RolesEnum.KitchenManager]
  },
  {
    name: 'Danh sách đơn hàng',
    url: '/order/list/tab4',
    icon: 'fa fa-list',
    roles: [RolesEnum.Checkfood]
  },
  {
    name: 'Danh sách đơn hàng',
    url: '/order/list/tab5',
    icon: 'fa fa-list',
    roles: [RolesEnum.Cashier]
  },
  {
    name: 'Thống kê doanh thu',
    url: '/statistic/cashierrevenue',
    icon: 'fa fa-dollar',
    roles: [RolesEnum.Admin, RolesEnum.KitchenManager]
  },

];
