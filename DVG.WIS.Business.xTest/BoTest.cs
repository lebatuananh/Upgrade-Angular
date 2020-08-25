using DVG.WIS.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace DVG.WIS.Business.xTest
{
    public class BoTest
    {
        public BoTest()
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."))
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .Build();
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            AppSettings.Instance.SetConfiguration(config);
        }
    }
}
