using AISystemGuardian.Models;

namespace AISystemGuardian.Service
{
    public class AnomalyDetectorService
    {
        private readonly Queue<SystemMetrics> _history = new Queue<SystemMetrics>();
        private const int MaxHistory = 10;

        public bool IsAnomaly(SystemMetrics current)
        {
            if (_history.Count < MaxHistory)
            {
                _history.Enqueue(current);
                return false;
            }

            var avgCpu = _history.Average(x => x.CpuUsage);
            var avgRam = _history.Average(x => x.RamUsage);

            // Detect sudden spikes
            bool cpuSpike = current.CpuUsage > avgCpu + 30;
            bool ramSpike = current.RamUsage > avgRam + 30;

            _history.Dequeue();
            _history.Enqueue(current);

            return cpuSpike || ramSpike;
        }
    }
}
