using System.Net;
using System.Text.Json;

namespace Api.Middleware
{
    public class GlobalErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public GlobalErrorHandlerMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlerMiddleware> logger)
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
            catch (Exception exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var correlationId = context.Items.TryGetValue(CorrelationIdMiddleware.CorrelationIdHeaderName, out var id)
                    ? id
                    : "N/A";

                _logger.LogError(exception, "Global error handler caught an exception. CorrelationId: '{correlationID}'.", correlationId);

                var error = JsonSerializer.Serialize(new
                {
                    message = $"Error. Correlation id: {correlationId}"
                });

                await response.WriteAsync(error);
            }
        }
    }
}
