using AISystemGuardian.Service;
class Program
{
    static void Main(string[] args)
    {
        var monitor = new SystemMonitorService();
        var logger = new LoggingService();

        while (true)
        {
            var data = monitor.GetMetrics();

            // Display
            Console.Clear();
            Console.WriteLine("=== AI System Guardian ===");
            Console.WriteLine($"CPU Usage: {data.CpuUsage}%");
            Console.WriteLine($"CPU Temp : {data.CpuTemperature}°C");
            Console.WriteLine($"RAM Usage: {data.RamUsage}%");
            Console.WriteLine($"Time     : {data.TimeStamp}");

            // Save to file
            logger.Log(data);

            Thread.Sleep(5000);
        }
    }
}