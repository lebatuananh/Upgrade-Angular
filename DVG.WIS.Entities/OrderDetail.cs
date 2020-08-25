using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Entities
{
    public class OrderDetail 
    {
        public int order_detail_id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public int origin_price { get; set; }
        public int price { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public string note { get; set; }
        public int status { get; set; }
        public int code { get; set; }

        // Customize
        public string product_name { get; set; }
    }
}
