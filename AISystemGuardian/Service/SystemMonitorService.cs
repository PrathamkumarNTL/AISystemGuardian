using AISystemGuardian.Models;
using LibreHardwareMonitor.Hardware;

namespace AISystemGuardian.Service
{
    public class SystemMonitorService
    {
        private Computer _computer;

        public SystemMonitorService()
        {
            _computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true
            };

            _computer.Open();
        }

        public SystemMetrics GetMetrics()
        {
            float cpuTemp = 0;

            foreach (var hardware in _computer.Hardware)
            {
                hardware.Update();

                foreach (var sensor in hardware.Sensors)
                {
                    if (sensor.Value == null)
                        continue;

                    // ✅ Show only CPU Temperature
                    if (sensor.SensorType == LibreHardwareMonitor.Hardware.SensorType.Temperature &&
                        sensor.Name.ToLower().Contains("package"))
                    {
                        Console.WriteLine($"🌡 CPU Temp: {sensor.Value} °C");
                    }

                    // ✅ Show only CPU Total Usage
                    if (sensor.SensorType == LibreHardwareMonitor.Hardware.SensorType.Load &&
                        sensor.Name.ToLower().Contains("total"))
                    {
                        Console.WriteLine($"⚙ CPU Total Load: {sensor.Value}%");
                    }
                }
            }

            return new SystemMetrics
            {
                CpuUsage = GetCpuUsage(),
                RamUsage = GetRamUsage(),
                CpuTemperature = cpuTemp,
                Timestamp = DateTime.Now
            };
        }

        private float GetCpuUsage()
        {
            return (float)new Random().NextDouble() * 100; 
        }

        private float GetRamUsage()
        {
            var info = new Microsoft.VisualBasic.Devices.ComputerInfo();
            float total = info.TotalPhysicalMemory;
            float available = info.AvailablePhysicalMemory;

            return ((total - available) / total) * 100;
        }
    }
}