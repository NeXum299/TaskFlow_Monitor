using Microsoft.AspNetCore.Http;
using Prometheus;
using TaskFlow_Monitor.Domain.Interfaces.Services;
namespace TaskFlow_Monitor.API.Middleware
{
    public class MetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMetricsService _metricsService;

        public MetricsMiddleware(RequestDelegate next, IMetricsService metricsService)
        {
            _next = next;
            _metricsService = metricsService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var method = context.Request.Method;
            var endpoint = context.Request.Path;

            var histogram = _metricsService.GetHttpRequerstDurationHistogram(
                method,
                endpoint.ToString());

            using (histogram.NewTimer())
            {
                try
                {
                    await _next(context);

                    _metricsService.RecordHttpRequest(method, endpoint.ToString(), context.Response.StatusCode);
                }
                catch (Exception)
                {
                    _metricsService.RecordHttpRequest(method, endpoint.ToString(), 500);
                    throw;
                }
            }
        }
    }
}
