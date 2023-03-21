using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Middleware_Example.Middlewares
{
    public class SecondMiddleware
    {
        private readonly RequestDelegate _next;

        public SecondMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            System.Diagnostics.Debug.WriteLine("Reaching 2nd middleware");

            await _next(context);
        }

    }

    public static class SecondMiddlewareExtension
    {
        public static IApplicationBuilder UseSecondMiddleware (this IApplicationBuilder app)
        {
            return app.UseMiddleware<SecondMiddleware>();
        }
    }
}
