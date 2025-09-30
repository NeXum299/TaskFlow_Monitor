using Prometheus;

namespace TaskFlow_Monitor.Domain.Interfaces.Services
{
    public interface IMetricsService
    {
        void RecordTaskCreated(string priority);
        void RecordTaskStatusChanged(string fromStatus, string toStatus);
        void RecordTaskUpdated();
        void RecordHttpRequest(string method, string endpoint, int statusCode);
        Histogram.Child GetHttpRequerstDurationHistogram(string method, string endpoint);
    }
}
