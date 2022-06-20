using Stacker.Commands;
using Stacker.DI;
using Stacker.Services;
using System.Windows.Input;

namespace Stacker.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region Binding properties

        public ViewModel HistoryViewModel { get; }
        public ViewModel IntervalsViewModel { get; }
        public ViewModel DesksViewModel { get; }

        #endregion

        #region Commands

        public ICommand OpenHeightSettings { get; }
        private void OnOpenHeightSettings()
        {
            _applicationWindowsService.Open(WindowType.HeightSettings);
        }

        #endregion

        private readonly IApplicationWindowsService _applicationWindowsService;
        private readonly IAutomationService _automationService;

        #region Methods

        public MainViewModel()
        {
            CommonContainer.Build();

            _applicationWindowsService = CommonContainer.Resolve<IApplicationWindowsService>();
            _automationService = CommonContainer.Resolve<IAutomationService>();

            _automationService.Enable();

            HistoryViewModel = new HistoryViewModel();
            IntervalsViewModel = new IntervalsViewModel();
            DesksViewModel = new DesksViewModel();

            OpenHeightSettings = new Command((obj) => OnOpenHeightSettings());
        }

        #endregion
    }
}
