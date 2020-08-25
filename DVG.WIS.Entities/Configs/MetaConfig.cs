using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Entities
{
    public class MetaConfig
    {
        public List<Meta> Meta { get; set; }
    }

    public class Meta
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
    }
}
