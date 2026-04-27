using AISystemGuardian.AI;
using Microsoft.ML;

namespace AISystemGuardian.Service
{
    public class AIModelService
    {
        public string Predict(ModelInput input)
        {
            int score = 0;

            // CPU
            if (input.CpuUsage > 85) score += 2;
            else if (input.CpuUsage > 60) score += 1;

            // RAM
            if (input.RamUsage > 85) score += 2;
            else if (input.RamUsage > 60) score += 1;

            // Temperature
            if (input.CpuTemperature > 80) score += 2;
            else if (input.CpuTemperature > 65) score += 1;

            // Devices
            if (input.Microphone == 1) score += 1;
            if (input.Camera == 1) score += 1;

            // Final decision
            if (score >= 6)
                return "Critical";
            else if (score >= 3)
                return "Warning";
            else
                return "Normal";
        }
    }
}
