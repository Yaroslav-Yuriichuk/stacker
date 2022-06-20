using Stacker.Interfaces;
using Stacker.Models.Timers;
using System;

namespace Stacker.Services
{
    public class AutomationService : IAutomationService
    {
        public bool IsEnabled { get; private set; }

        private readonly IBluetoothService _bluetoothService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly INotificationService _notificationService;
        private readonly IComputerModeMonitoringService _computerModeMonitoringService;

        private ISingleTickTimer _notifyAboutUpTimer;
        private ISingleTickTimer _notifyAboutDownTimer;
        private ISingleTickTimer _moveDeskAfterNotificationTimer;
        private ISingleTickTimer _snoozeTimer;

        #region Methods

        public AutomationService(IBluetoothService bluetoothService, IUserSettingsService userSettingsService,
            INotificationService notificationService, IComputerModeMonitoringService computerModeMonitoringService)
        {
            _bluetoothService = bluetoothService;
            _userSettingsService = userSettingsService;
            _notificationService = notificationService;
            _computerModeMonitoringService = computerModeMonitoringService;

            SetUpTimers();

            _bluetoothService.OnDeskConnected += StartAutomation;
            _bluetoothService.OnDeskDisconnected += StopAutomation;

            _bluetoothService.OnModeChanged += OnModeChanged;

            _notificationService.OnRejected += Snooze;

            _computerModeMonitoringService.OnSleepModeEntered += StopAutomation;
        }

        ~AutomationService()
        {
            _bluetoothService.OnDeskConnected -= StartAutomation;
            _bluetoothService.OnDeskDisconnected -= StopAutomation;

            _bluetoothService.OnModeChanged -= OnModeChanged;

            _notificationService.OnRejected -= Snooze;

            _computerModeMonitoringService.OnSleepModeEntered -= StopAutomation;
        }

        public void Enable()
        {
            IsEnabled = true;
            if (_bluetoothService.IsConnected)
            {
                StartNotifying();
            }
        }

        public void Disable()
        {
            IsEnabled = false;
            StopNotifying();
        }

        public void UpdateIntervalForStayMode(TimeSpan interval)
        {
            _notifyAboutDownTimer.UpdateInterval(interval - _userSettingsService.MoveDeskAfterNotificationInterval);
            _userSettingsService.UpdateIntervalForStayMode(interval);
        }

        public void UpdateIntervalForSitMode(TimeSpan interval)
        {
            _notifyAboutUpTimer.UpdateInterval(interval - _userSettingsService.MoveDeskAfterNotificationInterval);
            _userSettingsService.UpdateIntervalForSitMode(interval);
        }

        private void SetUpTimers()
        {
            _notifyAboutUpTimer = new SingleTickTimer(NotifyAboutUp,
                _userSettingsService.IntervalForSitMode - _userSettingsService.MoveDeskAfterNotificationInterval);
            _notifyAboutDownTimer = new SingleTickTimer(NotifyAboutDown,
                _userSettingsService.IntervalForStayMode - _userSettingsService.MoveDeskAfterNotificationInterval);

            _moveDeskAfterNotificationTimer = new SingleTickTimer(MoveDesk, _userSettingsService.MoveDeskAfterNotificationInterval);
            _snoozeTimer = new SingleTickTimer(NotifyAboutUp, _userSettingsService.SnoozeInterval);
        }

        private void NotifyAboutUp()
        {
            _notificationService.Send("Table will move UP in one minute");
            _moveDeskAfterNotificationTimer.Start();
        }

        private void NotifyAboutDown()
        {
            _notificationService.Send("Table will move DOWN in one minute");
            _moveDeskAfterNotificationTimer.Start();
        }

        private void MoveDesk()
        {
            if (_bluetoothService.ConnectedDesk.Mode == Mode.Stay)
            {
                _bluetoothService.MoveConnectedDeskDown();
                return;
            }

            _bluetoothService.MoveConnectedDeskUp();
        }

        private void Snooze()
        {
            TimeSpan timeElapsed = _moveDeskAfterNotificationTimer.TimeAfterStart;
            _moveDeskAfterNotificationTimer.Stop();

            _snoozeTimer.UpdateTick(
                _bluetoothService.ConnectedDesk.Mode == Mode.Stay ? NotifyAboutDown : NotifyAboutUp);
            _snoozeTimer.UpdateInterval(_userSettingsService.SnoozeInterval - timeElapsed);
            _snoozeTimer.Start();
        }

        private void StartAutomation()
        {
            if (IsEnabled)
            {
                StartNotifying();
            }
        }

        private void StopAutomation()
        {
            StopNotifying();
        }

        private void OnModeChanged()
        {
            StopNotifying();
            StartNotifying();
        }

        private void StartNotifying()
        {
            if (_bluetoothService.ConnectedDesk.Mode == Mode.Stay)
            {
                _notifyAboutDownTimer.Start();
                return;
            }

            _notifyAboutUpTimer.Start();
        }

        private void StopNotifying()
        {
            foreach (ITimer t in new ITimer[]
                { _notifyAboutUpTimer, _notifyAboutDownTimer, _moveDeskAfterNotificationTimer, _snoozeTimer })
            {
                t.Stop();
            }
        }

        #endregion
    }
}
