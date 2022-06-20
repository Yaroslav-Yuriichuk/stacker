using Stacker.Interfaces;
using Stacker.Models.Timers;
using System;

namespace Stacker.Services
{
    public class DayDataService : IDayDataService
    {
        public DayData DayData => _dayData;

        public event Action<DayData> OnTick;

        private ITimer _dayDataCounterTimer;
        private DayData _dayData;

        private readonly IBluetoothService _bluetoothService;
        private readonly IDayDataSavingService _dayDataSavingService;
        private readonly IComputerModeMonitoringService _computerModeMonitoringService;

        #region Methods

        public DayDataService(IBluetoothService bluetoothService, IDayDataSavingService dayDataSavingService,
            IComputerModeMonitoringService computerModeMonitoringService)
        {
            _bluetoothService = bluetoothService;
            _dayDataSavingService = dayDataSavingService;
            _computerModeMonitoringService = computerModeMonitoringService;

            _bluetoothService.OnDeskConnected += StartCollectingData;
            _bluetoothService.OnDeskDisconnected += StopCollectingData;

            _computerModeMonitoringService.OnSleepModeEntered += StopCollectingData;
            _computerModeMonitoringService.OnSleepModeExited += ReloadData;

            _dayData = _dayDataSavingService.LoadTodayData();
            
            SetUpTimers();
            StartCollectingData();
        }

        ~DayDataService()
        {
            _bluetoothService.OnDeskConnected -= StartCollectingData;
            _bluetoothService.OnDeskDisconnected -= StopCollectingData;

            _computerModeMonitoringService.OnSleepModeEntered -= StopCollectingData;
            _computerModeMonitoringService.OnSleepModeExited -= ReloadData;
        }

        private void SetUpTimers()
        {
            _dayDataCounterTimer = new BasicTimer(CollectData, new TimeSpan(0, 1, 0));
        }

        private void StartCollectingData()
        {
            _dayDataCounterTimer.Start();
        }

        private void StopCollectingData()
        {
            _dayDataCounterTimer.Stop();
        }

        private void CollectData()
        {
            if (!_bluetoothService.IsConnected) return;

            if (_bluetoothService.ConnectedDesk.Mode == Mode.Sit
                || _bluetoothService.ConnectedDesk.Mode == Mode.MovingUp)
            {
                _dayData.TimeSpentInSitMode += new TimeSpan(0, 1, 0);
                OnTick?.Invoke(DayData);
                return;
            }

            _dayData.TimeSpentInStayMode += new TimeSpan(0, 1, 0);
            OnTick?.Invoke(DayData);
        }

        private void ReloadData()
        {
            _dayData = _dayDataSavingService.LoadTodayData();
            OnTick?.Invoke(DayData);
        }

        #endregion
    }
}
