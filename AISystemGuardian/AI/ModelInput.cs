using Microsoft.ML.Data;

namespace AISystemGuardian.AI
{
    public class ModelInput
    {
        [LoadColumn(0)] public float CpuUsage { get; set; }
        [LoadColumn(1)] public float RamUsage { get; set; }
        [LoadColumn(2)] public float CpuTemperature { get; set; }
        [LoadColumn(3)] public float Microphone { get; set; }
        [LoadColumn(4)] public float Camera { get; set; }

        [LoadColumn(5)]
        [ColumnName("Label")]   // 🔥 IMPORTANT
        public string Label { get; set; }
    }
}
