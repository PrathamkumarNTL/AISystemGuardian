using AISystemGuardian.Models;

namespace AISystemGuardian.Service
{
    public class NotificationService
    {
        private NotifyIcon _notifyIcon;

        public NotificationService()
        {
            _notifyIcon = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                Visible = true
            };
        }

        public void ShowNotification(Alert alert)
        {
            _notifyIcon.BalloonTipTitle = "AI System Guardian";
            _notifyIcon.BalloonTipText = $"[{alert.Severity}] {alert.Message}";

            _notifyIcon.ShowBalloonTip(3000);
        }
    }
}