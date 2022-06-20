using Stacker.Commands;
using Stacker.DI;
using Stacker.Services;
using System;
using System.Windows.Input;

namespace Stacker.ViewModels
{
    public class IntervalsViewModel : ViewModel
    {
        #region Binding properties

        private TimeSpan _intervalForSitMode;
        public TimeSpan IntervalForSitMode
        {
            get => _intervalForSitMode;
            set
            {
                if (_intervalForSitMode == value) return;

                _intervalForSitMode = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _intervalForStayMode;
        public TimeSpan IntervalForStayMode
        {
            get => _intervalForStayMode;
            set
            {
                if (_intervalForStayMode == value) return;

                _intervalForStayMode = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand IncrementIntervalForSitMode { get; }
        private void OnIncrementIntervalForSitMode()
        {
            IntervalForSitMode += new TimeSpan(0, 1, 0);
            _automationService.UpdateIntervalForSitMode(IntervalForSitMode);
        }

        public ICommand DecrementIntervalForSitMode { get; }
        private void OnDecrementIntervalForSitMode()
        {
            IntervalForSitMode -= new TimeSpan(0, 1, 0);
            _automationService.UpdateIntervalForSitMode(IntervalForSitMode);
        }

        public ICommand IncrementIntervalForStayMode { get; }
        private void OnIncrementIntervalForStayMode()
        {
            IntervalForStayMode += new TimeSpan(0, 1, 0);
            _automationService.UpdateIntervalForStayMode(IntervalForStayMode);
        }

        public ICommand DecrementIntervalForStayMode { get; }
        private void OnDecrementIntervalForStayMode()
        {
            IntervalForStayMode -= new TimeSpan(0, 1, 0);
            _automationService.UpdateIntervalForStayMode(IntervalForStayMode);
        }

        #endregion

        #region Constants

        private readonly TimeSpan MinIntervalForSitMode = new TimeSpan(0, 2, 0);
        private readonly TimeSpan MaxIntervalForSitMode = new TimeSpan(0, 120, 0);
        private readonly TimeSpan MinIntervalForStayMode = new TimeSpan(0, 2, 0);
        private readonly TimeSpan MaxIntervalForStayMode = new TimeSpan(0, 120, 0);

        #endregion

        private readonly IUserSettingsService _userSettingsService;
        private readonly IAutomationService _automationService;

        public IntervalsViewModel()
        {
            _userSettingsService = CommonContainer.Resolve<IUserSettingsService>();
            _automationService = CommonContainer.Resolve<IAutomationService>();

            IntervalForSitMode = _userSettingsService.IntervalForSitMode;
            IntervalForStayMode = _userSettingsService.IntervalForStayMode;

            IncrementIntervalForSitMode = new Command((obj) => OnIncrementIntervalForSitMode(),
                (obj) => IntervalForSitMode < MaxIntervalForSitMode);
            DecrementIntervalForSitMode = new Command((obj) => OnDecrementIntervalForSitMode(),
                (obj) => IntervalForSitMode > MinIntervalForSitMode);

            IncrementIntervalForStayMode = new Command((obj) => OnIncrementIntervalForStayMode(),
                (obj) => IntervalForStayMode < MaxIntervalForStayMode);
            DecrementIntervalForStayMode = new Command((obj) => OnDecrementIntervalForStayMode(),
                (obj) => IntervalForStayMode > MinIntervalForStayMode);
        }
    }
}
