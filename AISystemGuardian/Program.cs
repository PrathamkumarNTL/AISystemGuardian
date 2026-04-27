using AISystemGuardian.AI;
using AISystemGuardian.Models;
using AISystemGuardian.Service;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var monitor = new SystemMonitorService();
        var logger = new LoggingService();
        var security = new SecurityMonitorService();
        var alertService = new AlertService();
        var notifier = new NotificationService();
        var voiceAlert = new VoiceService();
        var dataCollector = new DataCollectionService();
        var aiService = new AIModelService();
        var fusionService = new AIAlertFusionService();
        var anomalyDetector = new AnomalyDetectorService();

        while (true)
        {
            var data = monitor.GetMetrics();
            var deviceUsage = security.CheckDeviceUsage();
            var alerts = alertService.GenerateAlerts(data, deviceUsage);

            bool isAnomaly = anomalyDetector.IsAnomaly(data);

            Console.Clear();

            Console.WriteLine("=== AI System Guardian ===");
            Console.WriteLine($"CPU Usage: {data.CpuUsage}%");
            Console.WriteLine($"CPU Temp : {data.CpuTemperature}°C");
            Console.WriteLine($"RAM Usage: {data.RamUsage}%");
            Console.WriteLine($"Time     : {data.Timestamp}");

            Console.WriteLine("\n=== Security Alerts ===");

            foreach (var usage in deviceUsage)
            {
                Console.WriteLine($"⚠️ {usage.ProcessName} using:");

                if (usage.IsMicrophoneActive)
                    Console.WriteLine("   🎤 Microphone");

                if (usage.IsCameraActive)
                    Console.WriteLine("   📷 Camera");
            }

            logger.Log(data);

            // Optional (enable later if needed)
            // dataCollector.SaveData(data, deviceUsage, alerts);

            // =========================
            // AI PREDICTION
            // =========================

            var input = new ModelInput
            {
                CpuUsage = data.CpuUsage,
                RamUsage = data.RamUsage,
                CpuTemperature = data.CpuTemperature,
                Microphone = deviceUsage.Any(d => d.IsMicrophoneActive) ? 1f : 0f,
                Camera = deviceUsage.Any(d => d.IsCameraActive) ? 1f : 0f
            };

            var prediction = aiService.Predict(input);

            Console.WriteLine("\n=== AI Decision ===");
            Console.WriteLine($"AI says system is: {prediction}");

            // =========================
            // ANOMALY DETECTION
            // =========================

            Console.WriteLine("\n=== Anomaly Detection ===");

            if (isAnomaly)
            {
                Console.WriteLine("🚨 Unusual behavior detected!");

                // 👉 Add anomaly as alert
                alerts.Add(new Alert
                {
                    Severity = "Critical",
                    Message = "Anomaly detected in system behavior",
                    Timestamp = DateTime.Now
                });
            }
            else
            {
                Console.WriteLine("System behavior normal");
            }

            // =========================
            // AI + ALERT FUSION
            // =========================

            var fusedAlerts = fusionService.FuseAlerts(alerts, prediction);

            Console.WriteLine("\n=== Alerts ===");

            foreach (var alert in fusedAlerts)
            {
                Console.WriteLine($"[{alert.Severity}] {alert.Message}");

                notifier.ShowNotification(alert);
                voiceAlert.SpeakAlert(alert);
            }

            Thread.Sleep(5000);
        }
    }
}