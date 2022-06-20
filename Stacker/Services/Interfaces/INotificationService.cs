using System;

namespace Stacker.Services
{
    public interface INotificationService
    {
        public string CurrentMessage { get; }

        public event Action OnAccepted;
        public event Action OnRejected;

        public void Send(string message);

        public void Accept();
        public void Reject();
    }
}
