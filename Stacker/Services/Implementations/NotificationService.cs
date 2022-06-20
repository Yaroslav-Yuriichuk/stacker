using Stacker.Models.Timers;
using Stacker.Views;
using System;

namespace Stacker.Services
{
    public class NotificationService : INotificationService
    {
        public string CurrentMessage { get; set; }

        public event Action OnAccepted;
        public event Action OnRejected;

        private ISingleTickTimer _closeNotificationTimer;

        private Notification _notification;

        #region Constants

        private readonly TimeSpan KeepNotificationAliveInterval = new TimeSpan(0, 0, 25);

        #endregion

        #region Methods

        public NotificationService()
        {
            _closeNotificationTimer = new SingleTickTimer(CloseNotification, KeepNotificationAliveInterval);
        }

        ~NotificationService()
        {
            _closeNotificationTimer.Stop();
            CloseNotification();
        }

        public void Send(string message)
        {
            CurrentMessage = message;
            _notification = new Notification();
            _notification.Show();
            _closeNotificationTimer.Start();
        }

        public void Accept()
        {
            _closeNotificationTimer.Stop();
            CloseNotification();
            OnAccepted?.Invoke();
        }

        public void Reject()
        {
            _closeNotificationTimer.Stop();
            CloseNotification();
            OnRejected?.Invoke();
        }

        private void CloseNotification()
        {
            _notification.Close();
            _notification = null;
        }

        #endregion
    }
}
