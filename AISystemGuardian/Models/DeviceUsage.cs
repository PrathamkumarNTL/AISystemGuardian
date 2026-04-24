namespace AISystemGuardian.Models
{
    public class DeviceUsage
    {
        public string ProcessName { get; set; }
        public bool IsMicrophoneActive { get; set; }
        public bool IsCameraActive { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
