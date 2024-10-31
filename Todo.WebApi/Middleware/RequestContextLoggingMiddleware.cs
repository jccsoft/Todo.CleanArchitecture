﻿using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace Todo.WebApi.Middleware;

public class RequestContextLoggingMiddleware(RequestDelegate next)
{
    private const string _correlationIdHeaderName = "X-Correlation-Id";

    public Task Invoke(HttpContext context)
    {
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(context)))
        {
            return next.Invoke(context);
        }
    }

    private static string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(
            _correlationIdHeaderName,
            out StringValues correlationId);

        return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
    }
}
