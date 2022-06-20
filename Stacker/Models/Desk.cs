using Stacker.Interfaces;
using System;
using Windows.Devices.Enumeration;

namespace Stacker.Models
{
    public class Desk : IDesk
    {
        public event Action<ConnectionState> OnConnectionStateChanged;

        public string Id => _device.Id;
        public string Name => _device.Name;
        public ConnectionState ConnectionState
        {
            get => _connectionState;
            set
            {
                _connectionState = value;
                OnConnectionStateChanged?.Invoke(_connectionState);
            }
        }

        private DeviceInformation _device;
        private ConnectionState _connectionState = ConnectionState.Disconnected;

        public Desk(DeviceInformation device, ConnectionState connectionState = ConnectionState.Disconnected)
        {
            _device = device;
            _connectionState = connectionState;
        }
    }
}
