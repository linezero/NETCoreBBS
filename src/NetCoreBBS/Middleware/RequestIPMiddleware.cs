using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NLog;

namespace NetCoreBBS.Middleware
{
    public class RequestIPMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestIPMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var url = httpContext.Request.Path.ToString();
            if (!(url.Contains("/css") || url.Contains("/js") || url.Contains("/images") || url.Contains("/lib")))
            {
                _logger.Info($"Url:{url} IP:{httpContext.Connection.RemoteIpAddress.ToString()} 时间：{DateTime.Now}");
            }
            await _next(httpContext);
        }
    }


    public static class RequestIPMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestIPMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestIPMiddleware>();
        }
    }
}
