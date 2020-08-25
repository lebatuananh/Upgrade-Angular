using System;
using System.Collections.Specialized;
using System.Configuration;

namespace DVG.WIS.Utilities.Configs
{
    public class AvatarConfigs : NameValueCollection
    {
        private static NameValueCollection avatarConfigs = ConfigurationManager.GetSection("AvatarConfigs") as NameValueCollection;

        public static string Value(string name, string defaultValue = "")
        {
            var conf = ConfigurationManager.AppSettings["AvatarConfigs"];
            return avatarConfigs != null && avatarConfigs[name] != null ? avatarConfigs[name] : defaultValue;
        }
    }
}
