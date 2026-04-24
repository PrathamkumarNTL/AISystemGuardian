using AISystemGuardian.Models;
using System.Diagnostics;

namespace AISystemGuardian.Service
{
    public class SecurityMonitorService
    {
        public List<DeviceUsage> CheckDeviceUsage()
        {
            var result = new List<DeviceUsage>();

            foreach(var process in Process.GetProcesses())
            {
                try
                {
                    if (!string.IsNullOrEmpty(process.ProcessName))
                    {
                        //Basic detection logic (placeholder)
                        bool micActive = IsMicrophoneBeingUsed(process.ProcessName);
                        bool camActive = IsCameraBeingUsed(process.ProcessName);

                        if(micActive || camActive)
                        {
                            result.Add(new DeviceUsage
                            {
                                ProcessName = process.ProcessName,
                                IsMicrophoneActive = micActive,
                                IsCameraActive = camActive,
                                Timestamp = DateTime.Now
                            });
                        }
                    }
                }
                catch
                {
                    
                }
            }

            return result;
        }

        private bool IsMicrophoneBeingUsed(string processName)
        {
            string[] knownApps = { "zoom", "teams", "skype", "chrome", "discord" };

            return Array.Exists(knownApps, app => processName.ToLower().Contains(app));
        }

        private bool IsCameraBeingUsed(string processName)
        {
            string[] knownApps = { "zoom", "teams", "skype", "camera", "obs" };

            return Array.Exists(knownApps, app => processName.ToLower().Contains(app));
        }
    }
}
