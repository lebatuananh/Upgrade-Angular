using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DVG.WIS.PublicModel.AhaMove
{
    public class OrderResponse
    {
        public string order_id { get; set; }
        public string status { get; set; }
        public string shared_link { get; set; }
        public OrderDetailResponse order { get; set; }
    }

    public class OrderDetailResponse
    {
        public string _id { get; set; }
        public string currency { get; set; }
        public int user_main_account { get; set; }
        public int user_bonus_account { get; set; }
        public int total_pay { get; set; }
        public double distance { get; set; }
        public short duration { get; set; }
        public int distance_fee { get; set; }
        public int request_fee { get; set; }
        public int stop_fee { get; set; }
        public int vat_fee { get; set; }
        public int discount { get; set; }
    }

    public class OrderEstimateResponse
    {
        public double distance { get; set; }
        public short duration { get; set; }
        public int voucher_discount { get; set; }
        public int subtotal_price { get; set; }
        public int total_price { get; set; }
    }

    public class WebHookResponse
    {
        [JsonPropertyName("_id")]
        public string DeliveryOrderId { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("city_id")]
        public string CityId { get; set; }
        [JsonPropertyName("service_id")]
        public string ServiceId { get; set; }
        [JsonPropertyName("cancel_by_user")]
        public bool CancelByUser { get; set; }
        [JsonPropertyName("cancel_comment")]
        public string CancelComment { get; set; }
        [JsonPropertyName("path")]
        public List<PathResponse> Paths { get; set; }
    }

    public class PathResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
