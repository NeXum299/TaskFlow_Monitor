using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TaskFlow_Monitor.API.HealthChecks
{
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly HttpClient _httpClient;

        public ApiHealthCheck(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync("https://httpbin.org/delay/1", cancellationToken);

                return response.IsSuccessStatusCode
                    ? HealthCheckResult.Healthy("API is responding correctly")
                    : HealthCheckResult.Degraded($"API responded with status: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"API health check failed: {ex.Message}");
            }
        }
    }
}
