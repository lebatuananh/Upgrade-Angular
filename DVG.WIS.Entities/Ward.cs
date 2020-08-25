using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Entities
{
    [Serializable]
    public class Ward
    {
        public int ward_id { get; set; }
        public string ward_name { get; set; }
        public int district_id { get; set; }
        public string ward_alias { get; set; }
        public string ward_prefix { get; set; }
        public string ward_alias_new { get; set; }
    }
}
