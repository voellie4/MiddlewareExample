using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Middleware_Example.Middlewares
{
    public class ThirdMiddleware
    {
        private readonly RequestDelegate _next;

        public ThirdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            System.Diagnostics.Debug.WriteLine("Reaching 3rd middleware");

            await _next(context);
        }
    }

    public static class ThirdMiddlewareExtension
    {
        public static IApplicationBuilder UseThirdMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ThirdMiddleware>();
        }
    }
}
