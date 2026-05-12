using System.Text.Json;

namespace ProductManagement_V2.Infrastructuree.Middleware
{
    public class ResultStatusCodeMiddleware
    {
        private readonly RequestDelegate _next;

        public ResultStatusCodeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBody = context.Response.Body;

            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                responseBody.Seek(0, SeekOrigin.Begin);

                if (TryReadStatusCode(context.Response, responseBody, out var statusCode))
                {
                    context.Response.StatusCode = statusCode;
                }

                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBody);
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

        private static bool TryReadStatusCode(HttpResponse response, Stream body, out int statusCode)
        {
            statusCode = default;

            if (body.Length == 0 ||
                response.ContentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) != true)
            {
                return false;
            }

            try
            {
                using var document = JsonDocument.Parse(body);

                if (!document.RootElement.TryGetProperty("statusCode", out var statusCodeElement) ||
                    !statusCodeElement.TryGetInt32(out statusCode))
                {
                    return false;
                }

                return statusCode >= StatusCodes.Status100Continue &&
                       statusCode <= 599;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}
