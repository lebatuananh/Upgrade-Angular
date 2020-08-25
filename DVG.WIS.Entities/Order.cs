using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Entities
{
    public class Order 
    {
        public int order_id { get; set; }
        public Guid code { get; set; }
        public string order_code { get; set; }
        public int customer_id { get; set; }
        public string customer_name { get; set; }
        public string city_code { get; set; }
        public int district_id { get; set; }
        public int ward_id { get; set; }
        public int street_id { get; set; }
        public string delivery_address { get; set; }
        public string customer_phone { get; set; }
        public string customer_note { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; }
        public string created_by { get; set; }
        public string modified_by { get; set; }
        public string reason_note { get; set; }
        public int origin_price { get; set; }
        public int price { get; set; }
        public short status { get; set; }
        public short delivery_status { get; set; }
        public short order_type { get; set; }
        public short source_type { get; set; }
        public short request_type { get; set; }
        public DateTime? delivery_date { get; set; }
        public int ship_fee { get; set; }
        public string cashier_receive { get; set; }
        public string cashier_refund { get; set; }
        public string delivery_order_id { get; set; }
        public short delivery_estimate_time { get; set; }
        public int delivery_price { get; set; }
        public short max_cooking_time { get; set; }
        public short delivery_cancel_status { get; set; }
    }

    public class OrderCount
    {
        public Int64 count { get; set; }
        public int status { get; set; }
        public int requesttype { get; set; }
    }

    public class SuggestAddress
    {
        public string formatted_address { get; set; }
        public string address { get; set; }
        public double lng { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string ward { get; set; }
        public double lat { get; set; }
        public int district_id { get; set; }
    }

    public class CustomerAddress
    {
        public int address_id { get; set; }
        public string name { get; set; }
        public string city_code { get; set; }
        public int district_id { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public byte is_default { get; set; }
    }
}
