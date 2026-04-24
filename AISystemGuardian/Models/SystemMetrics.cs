namespace AISystemGuardian.Models
{
    public class SystemMetrics
    {
        public float CpuUsage { get; set; }
        public float CpuTemperature { get; set; }
        public float RamUsage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
