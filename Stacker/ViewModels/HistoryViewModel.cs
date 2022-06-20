using Stacker.DI;
using Stacker.Services;
using System;

namespace Stacker.ViewModels
{
    public class HistoryViewModel : ViewModel
    {
        #region Binding properties

        private TimeSpan _timeSpentInSitMode = TimeSpan.Zero;
        public TimeSpan TimeSpentInSitMode
        {
            get => _timeSpentInSitMode;
            set
            {
                if (_timeSpentInSitMode == value) return;

                _timeSpentInSitMode = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _timeSpentInStayMode = TimeSpan.Zero;
        public TimeSpan TimeSpentInStayMode
        {
            get => _timeSpentInStayMode;
            set
            {
                if (_timeSpentInStayMode == value) return;

                _timeSpentInStayMode = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private readonly IDayDataService _dayDataService;

        #region Methods

        public HistoryViewModel()
        {
            _dayDataService = CommonContainer.Resolve<IDayDataService>();
            _dayDataService.OnTick += UpdateData;
        }

        ~HistoryViewModel()
        {
            _dayDataService.OnTick -= UpdateData;
        }

        private void UpdateData(DayData dayData)
        {
            TimeSpentInSitMode = dayData.TimeSpentInSitMode;
            TimeSpentInStayMode = dayData.TimeSpentInStayMode;
        }

        #endregion
    }
}
