using Stacker.Commands;
using Stacker.DI;
using Stacker.Interfaces;
using Stacker.Models;
using Stacker.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Stacker.ViewModels
{
    public class DeskViewModel : ViewModel
    {
        #region Binding properties

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;

                _name = value;
                OnPropertyChanged();
            }
        }

        private ConnectionState _connectionState;
        public ConnectionState ConnectionState
        {
            get => _connectionState;
            set
            {
                if (_connectionState == value) return;

                _connectionState = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand ToggleConnection { get; private set; }

        private async Task OnToggleConnection()
        {
            if (ConnectionState == ConnectionState.Connected)
            {
                Console.WriteLine($"Disconnecting {_desk.Name}");
                await _bluetoothService.Disconnect();
                return;
            }

            if (ConnectionState == ConnectionState.Disconnected)
            {
                Console.WriteLine($"Connecting {_desk.Name}");
                await _bluetoothService.Connect(_desk);
            }
        }

        #endregion

        #region Properties

        public string Id => _desk.Id;

        #endregion

        private readonly IDesk _desk;

        private readonly IBluetoothService _bluetoothService;

        public DeskViewModel(IDesk desk)
        {
            _desk = desk;
            _bluetoothService = CommonContainer.Resolve<IBluetoothService>();

            Name = _desk.Name;
            ConnectionState = _desk.ConnectionState;
            ToggleConnection = new Command((p) => Task.Run(OnToggleConnection));

            _desk.OnConnectionStateChanged += OnConnectionStateChanged;
        }

        ~DeskViewModel()
        {
            _desk.OnConnectionStateChanged -= OnConnectionStateChanged;
        }

        #region Methods

        private void OnConnectionStateChanged(ConnectionState connectionState)
        {
            ConnectionState = connectionState;
        }

        #endregion
    }
}
