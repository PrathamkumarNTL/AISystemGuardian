using AISystemGuardian.Models;

namespace AISystemGuardian.Service
{
    public class LoggingService
    {
        private readonly string _filePath;

        public LoggingService()
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            _filePath = Path.Combine(folderPath, "system_metrics.csv");

            //create file with header if not exists
            if(!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "Timestamp,CPU,RAM,Temp\n");
            }
        }

        public void Log(SystemMetrics data)
        {
            string line = $"{data.Timestamp},{data.CpuUsage},{data.RamUsage},{data.CpuTemperature}";

            File.AppendAllText(_filePath, line + Environment.NewLine);
        }
    }
}
