using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.WIS.Core
{
    public class KillCrawlerHelper
    {
        public static DateTime startDateRun = DateTime.MinValue;

        public static bool Running = false;

        //public static List<string> QuickBlockIPDetected = new List<string>() { "198.178.120.135", "118.70.190.249", "103.24.244.68", "103.24.244.51" };

        public static List<string> QuickBlockIPDetected = AppSettings.Instance.GetString("QuickBlockIPDetected", "198.178.120.135;118.70.190.249;103.24.244.68;103.24.244.51").Split(';').ToList();

        public static Dictionary<string, CrawlerIPMonitor> IPCrawlerDetected = new Dictionary<string, CrawlerIPMonitor>();

        public static void Count(string Ip, string useragent)
        {
            try
            {
                if (Running)
                {

                    if (IPCrawlerDetected.ContainsKey(Ip))
                    {
                        var ipobj = IPCrawlerDetected[Ip];
                        ipobj.Count++;
                        IPCrawlerDetected[Ip] = ipobj;
                    }
                    else
                    {
                        IPCrawlerDetected.Add(Ip, new CrawlerIPMonitor() { Count = 1, StartRequest = DateTime.Now, StartUrl = useragent });

                        if (IPCrawlerDetected.Count > 2000)
                        {
                            IPCrawlerDetected.Clear();
                        }
                    }
                }
            }
            catch (Exception)
            {
                //
            }
        }

        public static string ToDes()
        {
            string ip = string.Empty;
            var lst = IPCrawlerDetected.OrderByDescending(obj => obj.Value.Count).ToList();

            foreach (var pair in lst)
            {
                ip += string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", pair.Key, pair.Value.Count, pair.Value.StartRequest.ToString("dd/MM/yyyy HH:mm:ss"), pair.Value.StartUrl);
            }

            if (startDateRun != DateTime.MinValue)
            {
                ip += string.Format("<table border=\"1\"><tr><td>{0}</td></tr></table>", (int)(DateTime.Now - startDateRun).TotalSeconds);
            }

            return string.Format("<html><body><table border=\"1\">{0}</table></body></html>", ip);
        }

        public static string ToListDenied()
        {
            string retval = string.Empty;

            foreach (var ip in QuickBlockIPDetected)
            {
                retval += string.Format("{0}<br/>", ip);
            }

            return string.Format("<html><body>{0}</body></html>", retval);
        }
    }

    public struct CrawlerIPMonitor
    {
        public int Count { get; set; }
        public DateTime StartRequest { get; set; }
        public string StartUrl { get; set; }
    }
}
