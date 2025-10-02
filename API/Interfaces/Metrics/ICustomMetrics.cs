using Prometheus;

namespace TaskFlow_Monitor.API.Interfaces.Metrics
{
    public interface ICustomMetrics
    {
        void RecordTaskCreated(string priority);
        void RecordHttpRequest(string method, string endpoint, int statusCode);
        Histogram.Child GetHttpRequestDurationHistogram(
            string method,
            string endpoint);
        void UpdateTasksByStatusMetrics(
            Dictionary<string, int> tasksByStatus);
    }
}