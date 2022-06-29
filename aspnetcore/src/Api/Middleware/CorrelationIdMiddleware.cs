namespace Api.Middleware
{
    /// <summary>
    /// Generates a new correlation id and stores in the response headers.
    /// 
    /// Correlation id is used for tracing the request through the web application,
    /// making bug tracking easier.
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;
        public const string CorrelationIdHeaderName = "X-Correlation-ID";


        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            bool correlationIdExistsInHeaders = context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationIdInHeader);

            // If CorrelationId exists already in the request headers, use it,
            // otherwise generate new one.
            string correlationId = correlationIdExistsInHeaders
                ? correlationIdInHeader
                : Guid.NewGuid().ToString();

            // Add the CorrelationId to the response headers.
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add(CorrelationIdHeaderName, correlationId);
                return Task.CompletedTask;
            });

            // Add the CorrelationId to the HttpContext's Items.
            context.Items.Add(CorrelationIdHeaderName, correlationId);

            var clientId = context.User?.Claims.FirstOrDefault(claim => claim.Type == "clientId");
            var organizationId = context.User?.Claims.FirstOrDefault(claim => claim.Type == "organizationid");
            _logger.LogInformation("Correlation id '{correlationID}' generated for '{clientId}' '{organizationId}'.", correlationId, clientId, organizationId);

            await _next(context);
        }
    }
}
