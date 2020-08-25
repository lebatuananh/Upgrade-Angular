using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Entities
{
    public class EnumInfo
    {
        public EnumInfo() { }
        public EnumInfo(short _value, string _name, string _description)
        {
            this.value = _value;
            this.name = _name;
            this.description = _description;
        }
        public short value { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
