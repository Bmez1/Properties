using Microsoft.Extensions.Primitives;

using Serilog.Context;

using static System.Net.Mime.MediaTypeNames;

namespace Properties.Api.Middleware
{
    public class RequestLoggingMiddleware(RequestDelegate next)
    {
        private const string CorrelationIdHeaderName = "CorrelationId";

        public Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty(CorrelationIdHeaderName, GetCorrelationId(context)))
            {
                return next.Invoke(context);
            }
        }

        private static string GetCorrelationId(HttpContext context)
        {
            context.Request.Headers.TryGetValue(
                CorrelationIdHeaderName,
                out StringValues correlationId);

            return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
        }
    }
}
