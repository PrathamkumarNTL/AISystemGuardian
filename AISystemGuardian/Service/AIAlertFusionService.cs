using AISystemGuardian.Models;

namespace AISystemGuardian.Service
{
    public class AIAlertFusionService
    {
        public List<Alert> FuseAlerts(List<Alert> alerts, string aiPrediction)
        {
            var finalAlerts = new List<Alert>();

            foreach (var alert in alerts)
            {
                // Case 1: AI says Normal → ignore warnings
                if (aiPrediction == "Normal" && alert.Severity == "Warning")
                    continue;

                // Case 2: AI says Critical → upgrade everything
                if (aiPrediction == "Critical")
                {
                    alert.Severity = "Critical";
                    alert.Message = "[AI UPGRADED] " + alert.Message;
                }

                // Case 3: AI says Warning → downgrade critical
                if (aiPrediction == "Warning" && alert.Severity == "Critical")
                {
                    alert.Severity = "Warning";
                    alert.Message = "[AI DOWNGRADED] " + alert.Message;
                }

                finalAlerts.Add(alert);
            }

            return finalAlerts;
        }
    }
}
