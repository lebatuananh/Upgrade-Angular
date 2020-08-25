using DVG.WIS.Business.Account;
using DVG.WIS.Business.Order;
using DVG.WIS.Business.Product;
using DVG.WIS.DAL.Account;
using DVG.WIS.DAL.District;
using DVG.WIS.DAL.Order;
using DVG.WIS.DAL.OrderDetail;
using DVG.WIS.DAL.OrderHistory;
using DVG.WIS.DAL.Product;
using DVG.WIS.DAL.ThirdPartyLog;
using DVG.WIS.DAL.Ward;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DVG.CK.OMS.PublicApi
{
    public class IoC
    {
        public static void RegisterTypes(IServiceCollection services)
        {
            //app services


            //IHttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //Account
            services.AddTransient<IAccountDal, AccountDal>();
            services.AddTransient<IAccountBo, AccountBo>();

            //Product
            services.AddTransient<IProductDal, ProductDal>();
            services.AddTransient<IProductBo, ProductBo>();

            //
            services.AddTransient<IOrderDetailDal, OrderDetailDal>();

            services.AddTransient<IOrderHistoryBo, OrderHistoryBo>();
            services.AddTransient<IOrderHistoryDal, OrderHistoryDal>();

            services.AddTransient<IThirdPartyLogBo, ThirdPartyLogBo>();
            services.AddTransient<IThirdPartyLogDal, ThirdPartyLogDal>();

            //Order
            services.AddTransient<IOrderDetailDal, OrderDetailDal>();
            services.AddTransient<IOrderDal, OrderDal>();
            services.AddTransient<IOrderBo, OrderBo>();

            //Districtt
            services.AddTransient<IDistrictDal, DistrictDal>();
            services.AddTransient<IDistrictBo, DistrictBo>();
            services.AddTransient<IDistrictBoCached, DistrictBoCached>();

            //Ward
            services.AddTransient<IWardDal, WardDal>();
            services.AddTransient<IWardBo, WardBo>();
            services.AddTransient<IWardBoCached, WardBoCached>();


            //District
            services.AddTransient<IDistrictDal, DistrictDal>();
            services.AddTransient<IDistrictBo, DistrictBo>();

            //Ward
            services.AddTransient<IWardDal, WardDal>();
            services.AddTransient<IWardBo, WardBo>();
        }
    }
}
