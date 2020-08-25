using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Entities
{
    public class OrderHistory
    {
        public int order_history_id { get; set; }
        public int order_id { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public string change_log { get; set; }
        public short status { get; set; }
        public short delivery_status { get; set; }
        public short request_type { get; set; }
    }
}
