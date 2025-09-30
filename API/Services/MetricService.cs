using Prometheus;
using TaskFlow_Monitor.Domain.Interfaces.Services;

namespace TaskFlow_Monitor.API.Services
{
    public class MetricsService : IMetricsService
    {
        private readonly Counter _tasksCreatedCounter = Metrics
            .CreateCounter(
                "tasks_created_total",
                "Total number of tasks created",
                new CounterConfiguration
                {
                    LabelNames = ["priority"]
                });

        private readonly Counter _tasksUpdatedCounter = Metrics
            .CreateCounter(
                "tasks_updated_total",
                "Total number of tasks updated");

        private readonly Counter _tasksStatusChangedCounter = Metrics
            .CreateCounter(
                "tasks_status_changed_total",
                "Total number of task status changes",
                new CounterConfiguration
                {
                    LabelNames = ["from_status", "to_status"]
                });

        private readonly Gauge _tasksByStatusGauge = Metrics
            .CreateGauge(
                "tasks_by_status",
                "Current number of tasks by status",
                new GaugeConfiguration
                {
                    LabelNames = ["status"]
                });

        private readonly Gauge _tasksByPriorityGauge = Metrics
            .CreateGauge(
                "tasks_by_priority",
                "Current number of tasks by priority",
                new GaugeConfiguration
                {
                    LabelNames = ["priority"]
                });

        private readonly Counter _httpRequestsCounter = Metrics
            .CreateCounter(
                "http_requests_total",
                "Total HTTP requests",
                new CounterConfiguration
                {
                    LabelNames = ["method", "endpoint", "status_code"]
                });

        private readonly Histogram _httpRequestDuration = Metrics
            .CreateHistogram(
                "http_request_duration_seconds",
                "HTTP request duration in seconds",
                new HistogramConfiguration
                {
                    LabelNames = ["method", "endpoint"],
                    Buckets = [0.1, 0.5, 1.0, 2.0, 5.0]
                });

        public void RecordTaskCreated(string priority)
        {
            _tasksCreatedCounter.WithLabels(priority).Inc();
            UpdateTasksGauges();
        }

        public void RecordTaskStatusChanged(string fromStatus, string toStatus)
        {
            _tasksStatusChangedCounter.WithLabels(fromStatus, toStatus).Inc();
            UpdateTasksGauges();
        }

        public void RecordTaskUpdated()
        {
            _tasksUpdatedCounter.Inc();
        }

        public void RecordHttpRequest(string method, string endpoint, int statusCode)
        {
            _httpRequestsCounter.WithLabels(method, endpoint, statusCode.ToString()).Inc();
        }

        public Histogram.Child GetHttpRequerstDurationHistogram(string method, string endpoint)
        {
            return _httpRequestDuration.WithLabels(method, endpoint);
        }

        public void UpdateTasksGauges(
            Dictionary<string, int> statusCounts = null!,
            Dictionary<string, int> priorityCounts = null!)
        {

        }
    }
}
