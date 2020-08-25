using Base.Logging.ExceptionCustom;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Base.Logging.LoggingMiddlewares
{
    public class NotFoundException : BaseCustomException
    {
        public NotFoundException(string message = "Cannot find object") : base(new List<string> { message },
            HttpStatusCode.NotFound)
        {
        }
    }
}
