using Stacker.Interfaces;
using Stacker.Models.Timers;
using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace Stacker.Services
{
    public partial class BluetoothService : IBluetoothService
    {
        private int _currentHeight = 0;

        #region Methods

        private void StartMonitoring()
        {
            _connectedDeskData.deskHeightCharacteristic.ValueChanged += DeskHeightChangedEventHandler;
            Task.Run(async () =>
            {
                await SendCommandToMoveUp();
                await SendCommandToMoveDown();
            });
            Console.WriteLine("Started monitoring");
        }

        private void StopMonitoring()
        {
            _connectedDeskData.deskHeightCharacteristic.ValueChanged -= DeskHeightChangedEventHandler;
        }

        private void DeskHeightChangedEventHandler(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var reader = DataReader.FromBuffer(args.CharacteristicValue);
            byte[] input = new byte[reader.UnconsumedBufferLength];
            reader.ReadBytes(input);

            const int RequiredLenght = 9;
            if (input.Length != RequiredLenght) return;

            _currentHeight = (input[4] * 256 + input[5]) / 10;
            ConnectedDesk.CurrentHeight = _currentHeight;

            PerformModeCheck();
        }

        private void PerformModeCheck()
        {
            Mode currentMode = DetermiteCurrentMode();

            if (ConnectedDesk.Mode == Mode.MovingUp || ConnectedDesk.Mode == Mode.MovingDown)
                return;

            UpdateModeIfChanged(currentMode);
        }

        #endregion
    }
}
