using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Entities
{
    [Serializable]
    public class District
    {
        public int district_id { get; set; }
        public string district_name { get; set; }
        public string city_code { get; set; }
        public string area { get; set; }
        public string district_alias { get; set; }
        public string district_prefix { get; set; }
        public int ship_fee { get; set; }
    }
}
