using AISystemGuardian.Models;
using LibreHardwareMonitor.Hardware;
using System.Diagnostics;
//using Microsoft.VisualBasic;

namespace AISystemGuardian.Service
{
    public class SystemMonitorService
    {
        private Computer _computer;
        private PerformanceCounter ramCounter;

        public SystemMonitorService()
        {
            _computer = new Computer
            {
                IsCpuEnabled = true,
                IsMemoryEnabled = true
            };

            _computer.Open();

            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public SystemMetrics GetMetrics()
        {
            float cpuUsage = 0;
            float cpuTemp = 0;
            float ramUsage = GetRamUsage();

            foreach(var hardware in _computer.Hardware)
            {
                if(hardware.HardwareType == HardwareType.Cpu)
                {
                    hardware.Update();
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("Total"))
                        {
                            cpuUsage = sensor.Value ?? 0;
                        }

                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            cpuTemp = sensor.Value ?? 0;
                        }
                    }
                }
            }

            return new SystemMetrics
            {
                CpuUsage = cpuUsage,
                CpuTemperature = cpuTemp,
                RamUsage = ramUsage,
                Timestamp = DateTime.Now
            };
        }

        private float GetRamUsage()
        {
            float availableMB = ramCounter.NextValue();

            float totalMB = GetTotalMemoryInMB();

            float used = totalMB - availableMB;

            return (used / totalMB) * 100;
        }

        private float GetTotalMemoryInMB()
        {
            var info = new PerformanceCounter("Memory", "Committed Bytes");
            float totalBytes = info.NextValue();

            return totalBytes / (1024 * 1024);
        }
    }
}
