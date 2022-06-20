using Stacker.Interfaces;
using Stacker.Models;
using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace Stacker.Services
{
    public partial class BluetoothService : IBluetoothService
    {
        public bool IsConnected => ConnectedDesk != null;

        public event Action OnDeskConnected;
        public event Action OnDeskDisconnected;

        public event Action OnModeChanged;

        public IConnectedDesk ConnectedDesk { get; private set; }

        private struct ConnectedDeskData
        {
            public BluetoothLEDevice device;
            public GattCharacteristic moveDeskCharacteristic;
            public GattCharacteristic deskHeightCharacteristic;

            public ConnectedDeskData(BluetoothLEDevice device,
                GattCharacteristic moveDeskCharacteristic, GattCharacteristic deskHeightCharacteristic)
            {
                this.device = device;
                this.moveDeskCharacteristic = moveDeskCharacteristic;
                this.deskHeightCharacteristic = deskHeightCharacteristic;
            }
        }

        private ConnectedDeskData _connectedDeskData;

        #region Constants

        private const string MoveDeskCharacteristic = "ff01";
        private const string DeskHeightCharacteristic = "ff02";

        #endregion

        private readonly IComputerModeMonitoringService _computerModeMonitoringService;

        #region Methods

        public BluetoothService(IUserSettingsService userSettingsService,
            IComputerModeMonitoringService computerModeMonitoringService)
        {
            _userSettingsService = userSettingsService;
            
            _computerModeMonitoringService = computerModeMonitoringService;
            _computerModeMonitoringService.OnSleepModeEntered += DisconnectIfConnected;
            _computerModeMonitoringService.OnSleepModeExited += TryReconnect;

            SetUpTimers();
        }

        ~BluetoothService()
        {
            _computerModeMonitoringService.OnSleepModeEntered -= DisconnectIfConnected;
            _computerModeMonitoringService.OnSleepModeExited -= TryReconnect;
        }

        public async Task Connect(IDesk desk)
        {
            await Disconnect();

            desk.ConnectionState = ConnectionState.Connecting;
            BluetoothLEDevice bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(desk.Id);
            
            GattCharacteristic moveTableCharacteristic = null;
            GattCharacteristic deskHeightCharacteristic = null;

            GattDeviceServicesResult serviceResult = await bluetoothLeDevice.GetGattServicesAsync();

            if (serviceResult.Status != GattCommunicationStatus.Success)
            {
                desk.ConnectionState = ConnectionState.Disconnected;
                return;
            }

            var services = serviceResult.Services;
            foreach (var service in services)
            {
                GattCharacteristicsResult charachterisicResult = await service.GetCharacteristicsAsync();

                if (charachterisicResult.Status == GattCommunicationStatus.Success)
                {
                    var characteristics = charachterisicResult.Characteristics;
                    foreach (var characteristic in characteristics)
                    {
                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;

                        if (properties.HasFlag(GattCharacteristicProperties.WriteWithoutResponse)
                            && characteristic.Uuid.ToString("N").Substring(4, 4) == MoveDeskCharacteristic)
                        {
                            moveTableCharacteristic = characteristic;
                        }

                        if (properties.HasFlag(GattCharacteristicProperties.Notify)
                            && characteristic.Uuid.ToString("N").Substring(4, 4) == DeskHeightCharacteristic)
                        {
                            deskHeightCharacteristic = characteristic;
                            GattCommunicationStatus status = await deskHeightCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                                GattClientCharacteristicConfigurationDescriptorValue.Notify);
                        }
                    }
                }
            }

            if (moveTableCharacteristic != null && deskHeightCharacteristic != null)
            {
                desk.ConnectionState = ConnectionState.Connected;
                ConnectedDesk = new ConnectedDesk(desk);
                _connectedDeskData = new ConnectedDeskData(bluetoothLeDevice, moveTableCharacteristic, deskHeightCharacteristic);

                StartMonitoring();

                OnDeskConnected?.Invoke();
            }
        }

        public async Task Disconnect()
        {
            if (!IsConnected) return;

            StopMonitoring();
            await ConnectedDesk.Disconnect();

            StopMonitoring();
            _connectedDeskData.device.Dispose();
            
            ConnectedDesk = null;

            OnDeskDisconnected?.Invoke();
        }

        private void DisconnectIfConnected()
        {
            Task.Run(Disconnect);
        }

        private void TryReconnect()
        {
            
        }

        #endregion
    }
}
