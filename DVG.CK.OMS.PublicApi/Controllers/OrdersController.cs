using DVG.WIS.Business.Order;
using DVG.WIS.Business.Product;
using DVG.WIS.Core;
using DVG.WIS.Core.Enums;
using DVG.WIS.DAL.District;
using DVG.WIS.DAL.Ward;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel.AhaMove;
using DVG.WIS.Utilities.Logs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DVG.CK.OMS.PublicApi.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private IOrderBo _orderBo;
        private IDistrictBoCached _districtBoCached;
        private IWardBoCached _wardBoCached;
        private IProductBo _productBo;
        private const string CommentStatus = "Auto cancel, no driver accepted";
        private const string Modifier = "AhaMoveWebHook";
        private const string CompleteStatus = "COMPLETED";

        public OrdersController(IOrderBo OrderBo, IDistrictBoCached districtBoCached, IWardBoCached wardBoCached, IProductBo productBo)
        {
            _orderBo = OrderBo;
            _districtBoCached = districtBoCached;
            _wardBoCached = wardBoCached;
            _productBo = productBo;
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult Get()
        {
            return Ok("Welcome to DVG Cloud Kitchen API");
        }

        // POST api/values
        [HttpPost("ahamove-callback")]
        public IActionResult AhaMoveCallback([FromBody] WebHookResponse responseData)
        {
            Logger.WriteLog(Logger.LogType.Info, "Start AhaMoveCallback: " + JsonConvert.SerializeObject(responseData));
            if (responseData == null || string.IsNullOrEmpty(responseData.DeliveryOrderId))
            {
                Logger.WriteLog(Logger.LogType.Error, "Data post null or DeliveryOrderId empty");
                return BadRequest();
            }
            /*WebHookResponse responseData;
            try
            {
                responseData = JsonConvert.DeserializeObject<WebHookResponse>(value);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return BadRequest();
            }*/
            var orderInfo = _orderBo.GetByDeliveryOrderId(responseData.DeliveryOrderId);
            // If order is not found in DB
            if (orderInfo == null)
            {
                Logger.WriteLog(Logger.LogType.Error, "Order from delivery API not found");
                return BadRequest();
            }
            // If current order is invalid
            if (orderInfo.status != (short)OrderStatusEnum.KitchenDone && orderInfo.status != (short)OrderStatusEnum.Delivering)
            {
                Logger.WriteLog(Logger.LogType.Error, string.Format("Order with ID {0} has status '{1}' is not 'Delivering'.", orderInfo.order_id, orderInfo.status));
                return BadRequest();
            }

            // Change to new delivery status
            var changeDeliveryStatus = CoreUtils.GetValueFromDescription<DeliveryStatus>(responseData.Status);
            if (changeDeliveryStatus == DeliveryStatus.Default || (short)changeDeliveryStatus == orderInfo.delivery_status)
            {
                Logger.WriteLog(Logger.LogType.Error, string.Format("Order with ID {0} hasn't change deleviry status.", orderInfo.order_id, orderInfo.status));
                return Ok();
            }

            // If CANCELLED status
            DeliveryCancelStatus cancelStatus;
            ErrorCodes result;
            switch (changeDeliveryStatus)
            {
                // Khi hoàn thành vận chuyển, cần kiểm tra trạng thái chính xác là Hoàn thành hay Bị hủy
                case DeliveryStatus.Completed:
                    OrderStatusEnum status;
                    string reason;
                    if (responseData.Paths.Count > 1 && responseData.Paths[1].Status == CompleteStatus)
                    {
                        status = OrderStatusEnum.Success;
                        reason = "Delivery SUCCESS";
                    }
                    else
                    {
                        status = OrderStatusEnum.Failure;
                        reason = "Delivery FAILED";
                    }

                    result = _orderBo.UpdateCompleteStatus(orderInfo.order_id, status, changeDeliveryStatus, reason, Modifier);
                    break;

                case DeliveryStatus.Cancelled:
                    Logger.WriteLog(Logger.LogType.Info, responseData.CancelComment);
                    if (responseData.CancelByUser)
                    {
                        cancelStatus = DeliveryCancelStatus.Customer;
                    }
                    else if (responseData.CancelComment == CommentStatus)
                    {
                        cancelStatus = DeliveryCancelStatus.Timeout;
                    }
                    else
                    {
                        cancelStatus = DeliveryCancelStatus.Supplier;
                    }
                    result = _orderBo.UpdateDeliveryStatus(orderInfo.order_id, (short)changeDeliveryStatus, (short)cancelStatus, Modifier);
                    break;

                default:
                    result = _orderBo.UpdateDeliveryStatus(orderInfo.order_id, (short)changeDeliveryStatus, 0, Modifier);
                    break;
            }

            dynamic response;
            if (result == ErrorCodes.Success)
            {
                response = new
                {
                    _id = responseData.DeliveryOrderId,
                    status = "SUCCESS"
                };
            }
            else
            {
                response = new
                {
                    _id = responseData.DeliveryOrderId,
                    status = "FAILURE"
                };
            }
            Logger.WriteLog(Logger.LogType.Info, "Finish AhaMoveCallback successfully");
            return Ok(response);
        }
    }
}