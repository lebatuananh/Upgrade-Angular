using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Entities
{
    public class ThirdPartyLog
    {
        public int third_party_log_id { get; set; }
        public int order_id { get; set; }
        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public int third_party_id { get; set; }
        public int http_status { get; set; }
        public string error_code { get; set; }
        public string message { get; set; }
        public int status { get; set; }
        public DateTime updated_date { get; set; }
        public string updated_date_by { get; set; }
    }
}
