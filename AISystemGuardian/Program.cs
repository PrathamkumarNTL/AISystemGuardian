using AISystemGuardian.Service;
class Program
{
    static void Main(string[] args)
    {
        var monitor = new SystemMonitorService();
        var logger = new LoggingService();
        var security  = new SecurityMonitorService();

        while (true)
        {
            var data = monitor.GetMetrics();
            var deviceUsage = security.CheckDeviceUsage();

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

            Thread.Sleep(5000);
        }
    }
}