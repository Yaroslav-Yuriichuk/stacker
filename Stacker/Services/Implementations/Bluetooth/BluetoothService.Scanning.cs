using Stacker.Interfaces;
using Stacker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace Stacker.Services
{
    public partial class BluetoothService : IBluetoothService
    {
        public event Action<IDesk> OnDeskAdded;
        public event Action<IDesk> OnDeskRemoved;

        private readonly List<IDesk> _desks = new List<IDesk>();

        private DeviceWatcher _watcher;

        #region Methods

        public void StartScanning()
        {
            if (_watcher == null) Initialize();
            _watcher.Start();
        }

        public void StopScanning()
        {
            _watcher.Stop();
        }

        private void Initialize()
        {
            string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

            _watcher = DeviceInformation.CreateWatcher(
                                BluetoothLEDevice.GetDeviceSelectorFromPairingState(false),
                                requestedProperties,
                                DeviceInformationKind.AssociationEndpoint);

            _watcher.Added += DeviceAdded;
            _watcher.Updated += DeviceUpdated;
            _watcher.Removed += DeviceRemoved;
        }

        private void DeviceAdded(DeviceWatcher sender, DeviceInformation args)
        {
            if (string.IsNullOrEmpty(args.Name) || string.IsNullOrWhiteSpace(args.Name))
                return;

            IDesk desk = new Desk(args);

            Console.WriteLine($"Found device: {desk.Name}");
            _desks.Add(desk);
            OnDeskAdded?.Invoke(desk);
        }

        private void DeviceUpdated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            IDesk desk = _desks.Find((d) => d.Id == args.Id);
            if (desk == null) return;

            Console.WriteLine($"Updated: {desk.Name}");
        }

        private async void DeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            IDesk desk = _desks.Find((d) => d.Id == args.Id);
            if (desk == null || desk.ConnectionState == ConnectionState.Connected) return;

            if (IsConnected && desk.Id == ConnectedDesk.Id) await Disconnect();

            Console.WriteLine($"Removed device: {desk.Name}");
            _desks.Remove(desk);
            OnDeskRemoved?.Invoke(desk);
        }

        #endregion
    }
}
