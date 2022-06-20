using Stacker.Commands;
using Stacker.DI;
using Stacker.Services;
using System.Windows.Input;

namespace Stacker.ViewModels
{
    public class NotificationViewModel : ViewModel
    {
        #region Binding properties

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                if (_message == value)
                    return;

                _message = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand Accept { get; }
        private void OnAccept()
        {
            _notificationService.Accept();
        }

        public ICommand Reject { get; }
        private void OnReject()
        {
            _notificationService.Reject();
        }

        #endregion

        private INotificationService _notificationService;

        public NotificationViewModel()
        {
            _notificationService = CommonContainer.Resolve<INotificationService>();

            Message = _notificationService.CurrentMessage;
            Accept = new Command((obj) => OnAccept());
            Reject = new Command((obj) => OnReject());
        }
    }
}
