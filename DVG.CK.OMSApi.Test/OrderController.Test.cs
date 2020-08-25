using DVG.CK.OMSApi.Controllers;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Order;
using DVG.WIS.Business.Product;
using DVG.WIS.DAL.District;
using DVG.WIS.DAL.Ward;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Net.WebSockets;

namespace DVG.CK.OMSApi.Test
{
    public class OrderControllerTest
    {
        private OrderController orderController;

        public OrderControllerTest()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile($"appsettings.{environmentName}.json", true)
            .Build();

            AppSettings.Instance.SetConfiguration(configuration);

            var service = new Mock<IServiceCollection>();
            IoC.RegisterTypes(service.Object);
            //var context = new Mock<IHttpContextAccessor>().Object;
            var orderBo = new Mock<IOrderBo>();
            var districtBoCached = new Mock<IDistrictBoCached>();
            var wardBoCached = new Mock<IWardBoCached>();
            var productBo = new Mock<IProductBo>();
            var userService = new Mock<IUserService>();
            orderController = new OrderController(orderBo.Object, districtBoCached.Object, wardBoCached.Object, productBo.Object, userService.Object);

        }

        //[SetUp]
        //public void Setup(string[] args)
        //{
        //    var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        //    var configuration = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.json", true)
        //    .AddJsonFile($"appsettings.{environmentName}.json", true)
        //    .Build();

        //    AppSettings.Instance.SetConfiguration(configuration);
        //}

        [Test]
        public void Index()
        {
            JsonResult result = orderController.Index();
            Assert.IsTrue(result != null);
            Assert.Pass(result.Value.ToString());
        }

        [Test]
        public void Search()
        {
            OrderSearchModel model = new OrderSearchModel();

            ActionResult result = orderController.Search(model);
            Assert.IsTrue(result != null);
            Assert.Pass(result.ToString());
        }
    }
}