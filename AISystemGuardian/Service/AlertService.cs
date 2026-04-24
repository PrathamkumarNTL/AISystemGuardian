using AISystemGuardian.Models;
using Mono.Unix.Native;

namespace AISystemGuardian.Service
{
    public class AlertService
    {
        public List<Alert> GenerateAlerts(SystemMetrics metrics,List<DeviceUsage> devices)
        {
            var alerts = new List<Alert>();

            // 🚨 CPU Alert
            if(metrics.CpuUsage > 85)
            {
                alerts.Add(new Alert 
                {
                    Message = "High CPU Usage Detected",
                    Severity = "High",
                    Timestamp = DateTime.Now
                });
            }

            // 🚨 RAM Alert
            if (metrics.RamUsage > 80)
            {
                alerts.Add(new Alert
                {
                    Message = "High RAM Usage Detected",
                    Severity = "Medium",
                    Timestamp = DateTime.Now
                });
            }


            // 🔐 Suspicious device usage
            foreach (var device in devices)
            {
                if(device.IsMicrophoneActive && !IsKnownApp(device.ProcessName))
                {
                    alerts.Add(new Alert
                    {
                        Message = $"Unknown app using microphone: {device.ProcessName}",
                        Severity = "High",
                        Timestamp = device.Timestamp
                    });
                }
            }

            // ⚡ Combined alert
            if(metrics.CpuUsage > 80 && devices.Count > 0)
            {
                alerts.Add(new Alert
                {
                    Message = "High CPU with active device usage — possible suspicious activity",
                    Severity = "High",
                    Timestamp = DateTime.Now
                });
            }

            return alerts;
        }

        private bool IsKnownApp(string processName)
        {
            string[] safeApps = { "chrome", "zoom", "teams", "discord", "skype" };

            return Array.Exists(safeApps, app => processName.ToLower().Contains(app));
        }
    }
}
