using AISystemGuardian.AI;
using AISystemGuardian.Service;
class Program
{
    
    static void Main(string[] args)
    {
        var monitor = new SystemMonitorService();
        var logger = new LoggingService();
        var security  = new SecurityMonitorService();
        var alertService = new AlertService();
        var notifier = new NotificationService();
        var voiceAlert = new VoiceService();
        var dataCollector = new DataCollectionService();
        var aiService = new AIModelService();
        var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "training_data.csv");
        //aiService.TrainModel(dataPath); ;
        var fusionService = new AIAlertFusionService();


        while (true)
        {
            var data = monitor.GetMetrics();
            var deviceUsage = security.CheckDeviceUsage();
            var alerts = alertService.GenerateAlerts(data, deviceUsage);

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

            //dataCollector.SaveData(data, deviceUsage, alerts);

            Console.WriteLine("\n=== Alerts ===");
            

            var input = new ModelInput
            {
                CpuUsage = data.CpuUsage,
                RamUsage = data.RamUsage,
                CpuTemperature = data.CpuTemperature,
                Microphone = deviceUsage.Any(d => d.IsMicrophoneActive) ? 1f : 0f,
                Camera = deviceUsage.Any(d => d.IsCameraActive) ? 1f : 0f
            };

            var prediction = aiService.Predict(input);

            var fusedAlerts = fusionService.FuseAlerts(alerts, prediction);

            foreach (var alert in fusedAlerts)
            {
                Console.WriteLine($"[{alert.Severity}] {alert.Message}");

                notifier.ShowNotification(alert);
                voiceAlert.SpeakAlert(alert);
            }

            Console.WriteLine($"\n=== AI Prediction ===");
            Console.WriteLine($"System State: {prediction}");

            Thread.Sleep(5000);
        }
    }
}