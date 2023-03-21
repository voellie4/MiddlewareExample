using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Middleware_Example.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        ILogger<ExceptionHandlingMiddleware> _logger = null;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                await HandleExceptionAsync(context, e);
            }

        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var exception = GetExceptionViewModel(ex);

            var result = JsonConvert.SerializeObject(exception);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            }

            return context.Response.WriteAsync(result);
        }

        public class ExceptionViewModel
        {
            public string ClassName { get; set; }
            public string Message { get; set; }
            public ExceptionViewModel InnerException { get; set; }
            public List<string> StackTrace { get; set; }
        }

        private ExceptionViewModel GetExceptionViewModel(Exception ex)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Microsoft.AspNetCore.Hosting.EnvironmentName.Development;

            return new ExceptionViewModel()
            {
                ClassName = !isDevelopment ? "Exception" : ex.GetType().Name.Split('.').Reverse().First(),
                InnerException = ex.InnerException != null ? GetExceptionViewModel(ex.InnerException) : null,
                Message = !isDevelopment ? "Internal Server Error" : ex.Message,
                StackTrace = !isDevelopment ? new List<string>() : ex.StackTrace.Split(Environment.NewLine).ToList()
            };
        }

    }
}
