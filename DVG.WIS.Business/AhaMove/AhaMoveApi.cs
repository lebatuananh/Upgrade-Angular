using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.DAL.Order;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel.AhaMove;
using DVG.WIS.Utilities;
using DVG.WIS.Utilities.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;

namespace DVG.WIS.Business.AhaMove
{
    public class AhaMoveApi
    {
        const string RegisterAccountPath = "{0}/v1/partner/register_account?mobile={3}&name={1}&api_key={2}";
        const string CreateOrderPath = "/v1/order/create";
        const string EstimateOrderFeePath = "/v1/order/estimated_fee";
        const string Address = "{{\"address\":\"{0}\",\"name\":\"{1}\",\"mobile\":\"{2}\"}}";
        const string Product = "{{\"_id\":\"{0}\",\"num\":{1},\"name\":\"{2}\",\"price\":{3}}}";
        const string CreateOrderParam = @"token={0}&order_time=0&path=[{1}]&service_id={2}&requests=[]&items=[{3}]";
        private static TokenInfo tokenInfo;

        /// <summary>
        /// The request token
        /// Author: ThanhDT
        /// Created date: 8/7/2020 2:33 PM
        /// </summary>
        /// <returns></returns>
        private static TokenInfo RequestToken()
        {
            var urlRequest = string.Format(RegisterAccountPath, StaticVariable.AhaMoveAPI, "Ahamove+Dai+Viet+Group", StaticVariable.AhaMoveKey, StaticVariable.Kitchen1Phone);
            HttpStatusCode statusCode;
            string message;
            var token = HttpRequestUtils.GetRequest(urlRequest, out statusCode, out message);
            try
            {
                tokenInfo = JsonConvert.DeserializeObject<TokenInfo>(token);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.ToString());
            }
            // Logging to LogSystem here

            return tokenInfo;
        }

        /// <summary>
        /// The create order
        /// Author: ThanhDT
        /// Created date: 8/7/2020 2:33 PM
        /// </summary>
        /// <param name="kitchenPath">The kitchen path.</param>
        /// <param name="customerPath">The customer path.</param>
        /// <param name="orderDetails">The order details.</param>
        /// <returns></returns>
        public static OrderResponse CreateOrder(PathInfo kitchenPath, PathInfo customerPath, List<OrderDetail> orderDetails)
        {
            // Nếu token null or empty
            if (tokenInfo == null)
            {
                tokenInfo = RequestToken();
            }
            if (tokenInfo == null)
            {
                return null;
            }

            // Build Path
            var kitchenInfo = string.Format(Address, kitchenPath.Address, kitchenPath.Name, kitchenPath.Mobile);
            var customerInfo = string.Format(Address, customerPath.Address, customerPath.Name, customerPath.Mobile);
            // Format product items
            var sbProduct = new StringBuilder();
            foreach (var orderDetail in orderDetails)
            {
                if (sbProduct.Length != 0) sbProduct.Append(",");
                sbProduct.AppendFormat(Product, orderDetail.product_id, orderDetail.quantity, orderDetail.product_name, orderDetail.price);
            }
            // Format parameters
            var parameters = string.Format(CreateOrderParam, HttpUtility.UrlEncode(tokenInfo.token), HttpUtility.UrlEncode(kitchenInfo + "," + customerInfo), StaticVariable.AhaMoveServiceId, HttpUtility.UrlEncode(sbProduct.ToString()));
            HttpStatusCode statusCode;
            string message;
            var response = HttpRequestUtils.PostRequest(StaticVariable.AhaMoveAPI + CreateOrderPath, parameters, out statusCode, out message);

            // If token expire then refresh token
            if (statusCode == HttpStatusCode.NotFound || statusCode == HttpStatusCode.NotAcceptable)
            {
                tokenInfo = RequestToken();
                if (tokenInfo == null)
                {
                    return null;
                }
                response = HttpRequestUtils.PostRequest(StaticVariable.AhaMoveAPI + CreateOrderPath, parameters, out statusCode, out message);
            }

            // Logging to LogSystem here

            // if response null
            if (string.IsNullOrEmpty(response)) return null;

            // Parse data
            OrderResponse orderResponse;
            try
            {
                orderResponse = JsonConvert.DeserializeObject<OrderResponse>(response);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.ToString());
                return null;
            }

            return orderResponse;
        }

        /// <summary>
        /// The create order
        /// Author: ThanhDT
        /// Created date: 8/7/2020 2:33 PM
        /// </summary>
        /// <param name="kitchenPath">The kitchen path.</param>
        /// <param name="customerPath">The customer path.</param>
        /// <returns></returns>
        public static OrderEstimateResponse EstimateOrderFee(PathInfo kitchenPath, PathInfo customerPath)
        {
            // Nếu token null or empty
            if (tokenInfo == null)
            {
                tokenInfo = RequestToken();
            }
            if (tokenInfo == null)
            {
                return null;
            }

            // Build Path
            var kitchenInfo = string.Format(Address, kitchenPath.Address, kitchenPath.Name, kitchenPath.Mobile);
            var customerInfo = string.Format(Address, customerPath.Address, customerPath.Name, customerPath.Mobile);

            // Format parameters
            var parameters = string.Format(CreateOrderParam, HttpUtility.UrlEncode(tokenInfo.token), HttpUtility.UrlEncode(kitchenInfo + "," + customerInfo), StaticVariable.AhaMoveServiceId, string.Empty);
            HttpStatusCode statusCode;
            string message;
            var response = HttpRequestUtils.PostRequest(StaticVariable.AhaMoveAPI + EstimateOrderFeePath, parameters, out statusCode, out message);

            // If token expire then refresh token
            if (statusCode == HttpStatusCode.NotFound || statusCode == HttpStatusCode.NotAcceptable)
            {
                tokenInfo = RequestToken();
                if (tokenInfo == null)
                {
                    return null;
                }
                response = HttpRequestUtils.PostRequest(StaticVariable.AhaMoveAPI + EstimateOrderFeePath, parameters, out statusCode, out message);
            }

            // Logging to LogSystem here

            // if response null
            if (string.IsNullOrEmpty(response)) return null;

            // Parse data
            OrderEstimateResponse orderEstimateResponse;
            try
            {
                orderEstimateResponse = JsonConvert.DeserializeObject<OrderEstimateResponse>(response);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.ToString());
                Console.WriteLine(ex.ToString());
                Console.WriteLine("->>>>>>>>>>>>>>>>>>>>>>>>>>>>>{0}", ex.Message);
                return null;
            }

            return orderEstimateResponse;
        }
    }
}
