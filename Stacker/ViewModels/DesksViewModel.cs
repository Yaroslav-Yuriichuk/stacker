using Stacker.Commands;
using Stacker.DI;
using Stacker.Extensions;
using Stacker.Interfaces;
using Stacker.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Stacker.ViewModels
{
    public class DesksViewModel : ViewModel
    {
        #region Binding properties

        private ObservableCollection<DeskViewModel> _desks = new ObservableCollection<DeskViewModel>();

        public ObservableCollection<DeskViewModel> Desks
        {
            get => _desks;
            set
            {
                if (_desks == value) return;

                _desks = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand ToggleMoveUp { get; }
        private void OnToggleMoveUp()
        {
            if (!_bluetoothService.IsConnected) return;

            if (_bluetoothService.ConnectedDesk.Mode == Mode.MovingUp)
            {
                _bluetoothService.StopMovingConnectedDeskUp();
                return;
            }

            _bluetoothService.MoveConnectedDeskUp();
        }

        public ICommand ToggleMoveDown { get; }
        private void OnToggleMoveDown()
        {
            if (!_bluetoothService.IsConnected) return;

            if (_bluetoothService.ConnectedDesk.Mode == Mode.MovingDown)
            {
                _bluetoothService.StopMovingConnectedDeskDown();
                return;
            }

            _bluetoothService.MoveConnectedDeskDown();
        }

        #endregion

        private readonly IBluetoothService _bluetoothService;

        #region Methods

        public DesksViewModel()
        {
            _bluetoothService = CommonContainer.Resolve<IBluetoothService>();

            ToggleMoveUp = new Command((obj) => OnToggleMoveUp(), (obj) => _bluetoothService.IsConnected);
            ToggleMoveDown = new Command((obj) => OnToggleMoveDown(), (obj) => _bluetoothService.IsConnected);

            _bluetoothService.OnDeskAdded += AddDesk;
            _bluetoothService.OnDeskRemoved += RemoveDesk;

            _bluetoothService.StartScanning();
        }

        ~DesksViewModel()
        {
            _bluetoothService.OnDeskAdded -= AddDesk;
            _bluetoothService.OnDeskRemoved -= RemoveDesk;
        }

        private void AddDesk(IDesk desk)
        {
            App.Current.Dispatcher.Invoke(() => {
                Desks.Add(new DeskViewModel(desk));
            });
        }

        private void RemoveDesk(IDesk desk)
        {
            App.Current.Dispatcher.Invoke(() => {
                DeskViewModel deskViewModel = Desks.Find<DeskViewModel>((d) => d.Id == desk.Id);
                Desks.Remove(deskViewModel);
            });
        }

        #endregion
    }
}
