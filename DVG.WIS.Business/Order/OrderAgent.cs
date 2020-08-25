using DVG.WIS.Business.AhaMove;
using DVG.WIS.Business.Order;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.DAL.District;
using DVG.WIS.DAL.Order;
using DVG.WIS.DAL.OrderDetail;
using DVG.WIS.DAL.OrderHistory;
using DVG.WIS.DAL.ThirdPartyLog;
using DVG.WIS.DAL.Ward;
using DVG.WIS.PublicModel.AhaMove;
using DVG.WIS.Utilities;
using DVG.WIS.Utilities.Logs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DVG.WIS.Business
{
    public class OrderAgent
    {
        private static bool IsRuning;
        private IOrderDal orderDal;
        private int OrderLimit = AppSettings.Instance.GetInt32("OrderLimit");
        // Hardcode temporary
        const int DeliveryTime = 45;

        public OrderAgent()
        {
            orderDal = new OrderDal();
        }

        /// <summary>
        /// The run
        /// Author: ThanhDT
        /// Created date: 8/9/2020 12:30 AM
        /// </summary>
        public void Run()
        {
            try
            {
                if (IsRuning) return;
                IsRuning = true;
                ProcessMoveOrderToKitchen();
                ProcessCallDelivery();
                IsRuning = false;
            }
            catch (Exception ex)
            {
                IsRuning = false;
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// The process data
        /// Author: ThanhDT
        /// Created date: 8/9/2020 2:22 AM
        /// </summary>
        private void ProcessMoveOrderToKitchen()
        {
            var pendingOrders = GetListPendingToKitchen(OrderLimit);
            if (pendingOrders.Count() == 0) return;

            //IOrderDal orderDal = new OrderDal();
            //IOrderDetailDal orderDetailDal = new OrderDetailDal();
            //IOrderHistoryBo orderHistoryBo = new OrderHistoryBo(new OrderHistoryDal());
            //IThirdPartyLogBo thirdPartyLogBo = new ThirdPartyLogBo(new ThirdPartyLogDal());
            //IOrderBo orderBo = new OrderBo(orderDal, orderDetailDal, orderHistoryBo, thirdPartyLogBo);
            foreach (var order in pendingOrders)
            {
                int deliveryTime;
                OrderEstimateResponse estimateOrder;
                // Nếu đơn hàng là Delivery thì cần tính toán thời gian vận chuyển
                if (order.order_type == (short)OrderTypeEnum.Delivery)
                {
                    PathInfo kitchenPath = new PathInfo { Address = StaticVariable.Kitchen1Address };
                    PathInfo customerPath = new PathInfo { Address = order.delivery_address, Name = order.customer_name, Mobile = order.customer_phone };
                    estimateOrder = AhaMoveApi.EstimateOrderFee(kitchenPath, customerPath);
                    if (estimateOrder == null)
                    {
                        estimateOrder = new OrderEstimateResponse();
                        deliveryTime = DeliveryTime;
                    }
                    else
                    {
                        deliveryTime = estimateOrder.duration / 60;
                    }
                }
                // Nếu đơn hàng là lấy ngay thì bỏ qua thời gian vận chuyển
                else
                {
                    deliveryTime = 0;
                    estimateOrder = new OrderEstimateResponse();
                }

                // Nếu thời gian giao hàng là NOW (=null) hoặc gần đến thời gian giao hàng bù cả thời gian nấu + vận chuyển thì chuyển sang bếp
                if (order.delivery_date == null || order.delivery_date.Value <= DateTime.Now.AddMinutes(order.max_cooking_time + deliveryTime))
                {
                    var orderInfo = new Entities.Order
                    {
                        order_id = order.order_id,
                        status = (short)OrderStatusEnum.PushToPOS,
                        //delivery_status = (short)DeliveryStatus.Idle,
                        //delivery_order_id = order.order_id,
                        delivery_price = estimateOrder.total_price,
                        delivery_estimate_time = (short)(estimateOrder.duration / 60),
                        reason_note = "MoveToKitchen"
                    };
                    var result = MoveOrderToChicken(orderInfo);
                    if (result != 1)
                    {
                        Logger.ErrorLog("[MoveOrderToChicken] Update error with orderId: " + order.order_id);
                    }
                }
            }
            //var orderList = new List<Entities.Order>
        }

        private void ProcessCallDelivery()
        {
            var pendingOrders = GetListToDelivery(OrderLimit);
            if (pendingOrders.Count() == 0) return;

            //IOrderDal orderDal = new OrderDal();
            IOrderDetailDal orderDetailDal = new OrderDetailDal();
            //IOrderHistoryBo orderHistoryBo = new OrderHistoryBo(new OrderHistoryDal());
            //IThirdPartyLogBo thirdPartyLogBo = new ThirdPartyLogBo(new ThirdPartyLogDal());
            //IOrderBo orderBo = new OrderBo(orderDal, orderDetailDal, orderHistoryBo, thirdPartyLogBo);
            foreach (var order in pendingOrders)
            {
                PathInfo kitchenPath = new PathInfo { Address = StaticVariable.Kitchen1Address };
                PathInfo customerPath = new PathInfo { Address = order.delivery_address, Name = order.customer_name, Mobile = order.customer_phone };
                var orderDetails = orderDetailDal.GetByOrderId(order.order_id);
                var orderResponse = AhaMoveApi.CreateOrder(kitchenPath, customerPath, orderDetails.ToList());
                if (orderResponse != null)
                {
                    var orderInfo = new Entities.Order
                    {
                        order_id = order.order_id,
                        //status = (short)OrderStatusEnum.Delivering,
                        delivery_status = (short)DeliveryStatus.Idle,
                        delivery_order_id = orderResponse.order_id,
                        delivery_price = orderResponse.order.total_pay,
                        delivery_estimate_time = orderResponse.order.duration,
                        reason_note = "ChangeToDelivery"
                    };
                    var result = ChangeOrderToDelivery(orderInfo);
                    if (result != 1)
                    {
                        Logger.ErrorLog("[ChangeToDelivery] Error save Delivery with OrderId: " + order.order_id);
                    }
                    else
                    {
                        Logger.WriteLog(Logger.LogType.Info, "[ChangeToDelivery] SUCCESS save Delivery with OrderId: " + order.order_id);
                    }
                }
                else
                {
                    var orderInfo = new Entities.Order
                    {
                        order_id = order.order_id,
                        //status = (short)OrderStatusEnum.Delivering,
                        delivery_status = (short)DeliveryStatus.ApiError,
                        delivery_order_id = string.Empty,
                        delivery_price = 0,
                        delivery_estimate_time = 0,
                        reason_note = "MarkDeliveryErrorApi"
                    };
                    var result = ChangeOrderToDelivery(orderInfo);
                    if (result != 1)
                    {
                        Logger.ErrorLog("[ChangeToDelivery] Error change DeliveryStatus to ApiError with orderId: " + order.order_id);
                    }
                    else
                    {
                        Logger.WriteLog(Logger.LogType.Info, "[ChangeToDelivery] Success change DeliveryStatus to ApiError with OrderId: " + order.order_id);
                    }
                }
            }
        }

        /// <summary>
        /// The move order to chicken
        /// Author: ThanhDT
        /// Created date: 8/9/2020 2:23 AM
        /// </summary>
        /// <param name="order">The order.</param>
        private int MoveOrderToChicken(Entities.Order order)
        {
            int result;
            try
            {
                result = orderDal.MoveToKitchen(order);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return 0;
            }
            return result;
        }

        /// <summary>
        /// The change to delivery
        /// Author: ThanhDT
        /// Created date: 8/12/2020 8:56 AM
        /// </summary>
        /// <param name="order">The order.</param>
        private int ChangeOrderToDelivery(Entities.Order order)
        {
            int result;
            try
            {
                result = orderDal.ChangeToDelivery(order);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return 0;
            }
            return result;
        }

        /// <summary>
        /// The get list pending to kitchen
        /// Author: ThanhDT
        /// Created date: 8/9/2020 2:23 AM
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        private IEnumerable<Entities.Order> GetListPendingToKitchen(int limit)
        {
            try
            {
                var result = orderDal.GetListPendingToKitchen((short)OrderStatusEnum.Pending, limit);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new List<Entities.Order>();
            }
        }

        /// <summary>
        /// The get list to delivery
        /// Author: ThanhDT
        /// Created date: 8/11/2020 4:50 PM
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        private IEnumerable<Entities.Order> GetListToDelivery(int limit)
        {
            try
            {
                var result = orderDal.GetListToDelivery((short)OrderStatusEnum.KitchenDone, limit);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new List<Entities.Order>();
            }
        }
    }
}
