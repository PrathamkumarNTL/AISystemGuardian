using AISystemGuardian.Models;
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

            Console.WriteLine("\n=== Alerts ===");
            foreach (var alert in alerts)
            {
                Console.WriteLine($"[{alert.Severity}] {alert.Message}");

                notifier.ShowNotification(alert);
            }

            notifier.ShowNotification(new Alert
            {
                Message = "Test Notification",
                Severity = "Info",
                Timestamp = DateTime.Now
            });

            Thread.Sleep(5000);
        }
    }
}