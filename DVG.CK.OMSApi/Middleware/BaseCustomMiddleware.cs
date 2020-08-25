using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DVG.CK.OMSApi.Middleware
{
    public abstract class BaseCustomMiddleware
    {
        protected readonly RequestDelegate next;

        public BaseCustomMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public abstract Task InvokeAsync(HttpContext context);

        public async Task WriteExceptionResponse(HttpContext context, ErrorModel errorModel)
        {
            await WriteJsonResult(context, (HttpStatusCode)errorModel.StatusCode, errorModel).ConfigureAwait(false);
        }

        public async Task WriteJsonResult(HttpContext context, HttpStatusCode statusCode, object obj)
        {
            context.Response.StatusCode = (int)statusCode;
            if (context.Response.Headers != null)
                context.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await context.Response.WriteAsync(JsonConvert.SerializeObject(obj, NewtonJsonSerializerSettings.CAMEL)).ConfigureAwait(false);
        }

        public async Task WriteImageResult(HttpContext context, string file)
        {
            await context.Response.SendFileAsync(file, 0, null).ConfigureAwait(false);
        }
    }
}
