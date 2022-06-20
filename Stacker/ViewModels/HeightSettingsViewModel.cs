using Stacker.Commands;
using Stacker.DI;
using Stacker.Services;
using System;
using System.Windows.Input;

namespace Stacker.ViewModels
{
    public class HeightSettingsViewModel : ViewModel
    {
        #region Binding properties

        private int _heightInSitMode;
        public int HeightInSitMode
        {
            get => _heightInSitMode;
            set
            {
                if (_heightInSitMode == value)
                    return;

                _heightInSitMode = value;
                OnPropertyChanged();
            }
        }

        private int _heightInStayMode;
        public int HeightInStayMode
        {
            get => _heightInStayMode;
            set
            {
                if (_heightInStayMode == value)
                    return;

                _heightInStayMode = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand ApplyChanges { get; }
        private void OnApplySettings()
        {
            Console.WriteLine($"Sit: {HeightInSitMode}, Stay: {HeightInStayMode}");

            _userSettingsService.UpdateHeightInSitMode(HeightInSitMode);
            _userSettingsService.UpdateHeightInStayMode(HeightInStayMode);
            
            _applicationWindowsService.Close(WindowType.HeightSettings);
        }

        #endregion

        private readonly IApplicationWindowsService _applicationWindowsService;
        private readonly IUserSettingsService _userSettingsService;

        #region Methods

        public HeightSettingsViewModel()
        {
            _applicationWindowsService = CommonContainer.Resolve<IApplicationWindowsService>();
            _userSettingsService = CommonContainer.Resolve<IUserSettingsService>();

            HeightInSitMode = _userSettingsService.HeightInSitMode;
            HeightInStayMode = _userSettingsService.HeightInStayMode;

            ApplyChanges = new Command((obj) => OnApplySettings());
        }

        #endregion
    }
}
