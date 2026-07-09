using Serilog.Context;
using System.Diagnostics;

namespace ShopApi.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;


    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }


    public async Task Invoke(HttpContext context)
    {
        var correlationId = Guid.NewGuid().ToString();//req id on req pipline

        var stopwatch = Stopwatch.StartNew();// rate req




      //  context.Response.Headers.Add("X-Correlation-Id", correlationId);

        using (LogContext.PushProperty(
            "CorrelationId",
            correlationId))
        {
            //first log start req
            _logger.LogInformation(
                "Request Started {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                //end req
                _logger.LogInformation(
                    "Request Finished {Method} {Path} {StatusCode} took {Elapsed} ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}