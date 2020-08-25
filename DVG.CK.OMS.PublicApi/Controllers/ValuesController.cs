using DVG.WIS.Utilities.Logs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVG.CK.OMS.PublicApi.Controllers
{
    [Route("api/values")]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            try
            {
                int i = 1 / int.Parse("0");
            }
            catch (Exception ex)
            {
                logger.LogInformation("Thanh Dai ca");
                Logger.WriteLog(Logger.LogType.Error, "Order from delivery API not found");
            }
            return "Value1";
        }
    }
}
