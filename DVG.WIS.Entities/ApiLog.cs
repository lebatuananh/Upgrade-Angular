using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Entities
{
    public class ApiLog
    {
        public int api_log_id { get; set; }
        public int order_id { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public int api_type { get; set; }
        public string message { get; set; }
        public short status { get; set; }
    }
}
