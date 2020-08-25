using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace DVG.WIS.Utilities.Configs
{
    public class MetaConfigs : NameValueCollection
    {
        private static NameValueCollection metaConfigs = ConfigurationManager.GetSection("MetaConfigs") as NameValueCollection;

        public static string Value(string name, string defaultValue = "")
        {
            return metaConfigs != null && metaConfigs[name] != null ? metaConfigs[name] : defaultValue;
        }
    }
}
