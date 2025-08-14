using System.Net;
using System.Text.Json;

namespace ProductAPI.API.Middlewares
{
    public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "NotFound: {Message}", ex.Message);
                await WriteProblem(context, ex, (int)HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
                await WriteProblem(context, ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        private static async Task WriteProblem(HttpContext context, Exception ex, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var errorResponse = new
            {
                message = "An unexpected error occurred.",
                detail = ex.Message,
                exceptionType = ex.GetType().FullName,
                stackTrace = ex.StackTrace,
                innerException = ex.InnerException?.ToString(),
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
