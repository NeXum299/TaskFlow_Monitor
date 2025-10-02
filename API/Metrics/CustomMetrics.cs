using Prometheus;
using TaskFlow_Monitor.API.Interfaces.Metrics;

namespace TaskFlow_Monitor.API.Metrics
{
    public class CustomMetrics : ICustomMetrics
    {
        private readonly Counter _httpRequestsCounter
            = Prometheus.Metrics.CreateCounter(
                "http_requests_total",
                "Total HTTP requests",
                new CounterConfiguration
                {
                    LabelNames = ["endpoint", "method", "status_code"]
                });

        private readonly Counter _tasksCreatedCounter
            = Prometheus.Metrics.CreateCounter(
                "tasks_created_total",
                "Total number of tasks created",
                new CounterConfiguration
                {
                    LabelNames = ["priority"]
                });

        private readonly Histogram _httpRequestDuration
            = Prometheus.Metrics.CreateHistogram(
                "http_request_duration_seconds",
                "HTPP request duration in seconds",
                new HistogramConfiguration
                {
                    LabelNames = ["method", "endpoint"],
                    Buckets = [0.1, 0.5, 1.0, 2.0, 5.0]
                });

        private readonly Gauge _tasksByStatusGauge
            = Prometheus.Metrics.CreateGauge(
                "tasks_by_status",
                "Current number of tasks by status",
                new GaugeConfiguration
                {
                    LabelNames = ["status"]
                });

        public void UpdateTasksByStatusMetrics(
            Dictionary<string, int> tasksByStatus)
        {
            var allKnownStatuses = new[] { "Pending",
                "InProgress", "Completed", "Cancelled" };
            
            foreach (var status in allKnownStatuses)
            {
                var gauge = _tasksByStatusGauge
                    .WithLabels(status);
                    
                if (tasksByStatus.ContainsKey(status))
                    gauge.Set(tasksByStatus[status]);
                else gauge.Set(0);
            }
        }

        public void RecordTaskCreated(string priority)
        {
            _tasksCreatedCounter
                .WithLabels(priority).Inc();
        }

        public void RecordHttpRequest(
            string method,
            string endpoint,
            int statusCode)
        {
            _httpRequestsCounter
                .WithLabels(
                    endpoint,
                    method,
                    statusCode.ToString())
                .Inc();
        }

        public Histogram.Child GetHttpRequestDurationHistogram(
            string method,
            string endpoint)
        {
            return _httpRequestDuration.WithLabels(method, endpoint);
        }
    }
}
