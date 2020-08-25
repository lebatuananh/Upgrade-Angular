using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Entities
{
    public class Product
    {
        public int product_id { get; set; }
        public int code { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string description { get; set; }
        public int category_id { get; set; }
        public DateTime created_date { get; set; }
        public DateTime modified_date { get; set; }
        public string created_by { get; set; }
        public string modified_by { get; set; }
        public int price { get; set; }
        public int origin_price { get; set; }
        public short status { get; set; }
        public short cooking_time { get; set; }
    }
}
