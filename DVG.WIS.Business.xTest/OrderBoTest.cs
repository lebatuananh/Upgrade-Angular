using DVG.WIS.Business.Order;
using DVG.WIS.DAL.Order;
using DVG.WIS.DAL.OrderDetail;
using DVG.WIS.DAL.OrderHistory;
using DVG.WIS.DAL.ThirdPartyLog;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DVG.WIS.Business.xTest
{
    public class OrderBoTest : BoTest
    {
        private IOrderBo _orderBo;
        //public OrderBoTest()
        //{
        //    _orderBo = new OrderBo(new OrderDal()
        //        , new OrderDetailDal()
        //        , new OrderHistoryBo(new OrderHistoryDal())
        //        , new ThirdPartyLogBo( new ThirdPartyLogDal()));
        //}
        [Fact]
        public void GetCashierRevenueStatistic()
        {
            //var fromDate = DateTime.Now.AddDays(-14);
            //var toDate = DateTime.Now;
            //var result = _orderBo.GetCashierRevenueStatistic(fromDate, toDate);
            //Console.WriteLine(result);
        }
    }
}
