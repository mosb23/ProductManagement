using System.Text.Json;
using ProductManagement_V2.Application.Common;

namespace ProductManagement_V2.Infrastructuree.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            IWebHostEnvironment environment,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _environment = environment;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unhandled exception while processing {Method} {Path}.",
                    context.Request.Method,
                    context.Request.Path);

                if (context.Response.HasStarted)
                {
                    throw;
                }

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var errors = _environment.IsDevelopment()
                    ? new List<string> { exception.Message }
                    : new List<string>();

                var response = ApiResponse<object>.FailResponse(
                    "Something went wrong on the server. Please try again later.",
                    StatusCodes.Status500InternalServerError,
                    errors);

                await JsonSerializer.SerializeAsync(
                    context.Response.Body,
                    response,
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
            }
        }
    }
}
