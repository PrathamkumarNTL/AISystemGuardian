using AISystemGuardian.Models;

namespace AISystemGuardian.Service
{
    public class DataCollectionService
    {
        private string filePath = "training_data.csv";

        public void SaveData(SystemMetrics metrics, List<DeviceUsage> devices, List<Alert> alerts)
        {
            bool mic = devices.Exists(d => d.IsMicrophoneActive);
            bool cam = devices.Exists(d => d.IsCameraActive);

            string label = GetLabel(alerts);

            string header = "CpuUsage,RamUsage,CpuTemperature,Microphone,Camera,Label";

            // ✅ Check if header exists
            if (!File.Exists(filePath) || !File.ReadAllText(filePath).StartsWith(header))
            {
                var existingData = File.Exists(filePath) ? File.ReadAllText(filePath) : "";

                File.WriteAllText(filePath, header + Environment.NewLine + existingData);
            }

            string line = $"{metrics.CpuUsage},{metrics.RamUsage},{metrics.CpuTemperature},{mic},{cam},{label}";

            File.AppendAllText(filePath, line + Environment.NewLine);
        }

        private string GetLabel(List<Alert> alerts)
        {
            if (alerts.Exists(a => a.Severity == "High"))
                return "Critical";

            if (alerts.Exists(a => a.Severity == "Medium"))
                return "Warning";

            return "Normal";
        }
    }
}
