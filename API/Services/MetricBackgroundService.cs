using TaskFlow_Monitor.API.Interfaces.Metrics;

namespace TaskFlow_Monitor.API.Services
{
    public class MetricsBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MetricsBackgroundService> _logger;

        public MetricsBackgroundService(
            IServiceProvider serviceProvider, 
            ILogger<MetricsBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var metricsService = scope.ServiceProvider.GetRequiredService<ITaskMetricsService>();
                        await metricsService.UpdateTasksByStatusMetricsAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating task metrics");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
    }