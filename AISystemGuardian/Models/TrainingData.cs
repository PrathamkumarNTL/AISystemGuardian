namespace AISystemGuardian.Models
{
    public class TrainingData
    {
        public float CpuUsage { get; set; }
        public float RamUsage { get; set; }
        public float CpuTemperature { get; set; }
        public bool IsMicrophoneActive { get; set; }
        public bool IsCameraActive { get; set; }
        public string Label { get; set; } 
    }
}
