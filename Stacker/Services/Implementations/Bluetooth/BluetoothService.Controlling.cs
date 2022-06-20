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
        private ITimer _sendCommandUpTimer;
        private ITimer _sendCommandDownTimer;

        private readonly IUserSettingsService _userSettingsService;

        #region Constants

        private readonly byte[] MoveTabeUpCommand = { 0xF1, 0xF1, 0x01, 0x00, 0x01, 0x7E };
        private readonly byte[] MoveTableDownCommand = { 0xF1, 0xF1, 0x02, 0x00, 0x02, 0x7E };

        private readonly TimeSpan SendCommandInterval = new TimeSpan(0, 0, 0, 0, 200);

        #endregion

        #region Methods

        public void MoveConnectedDeskUp()
        {
            StopMovingConnectedDeskDown();
            if (!IsConnected || ConnectedDesk.Mode == Mode.Stay) return;

            UpdateModeIfChanged(Mode.MovingUp);
            _sendCommandUpTimer.Start();
            Console.WriteLine("Started moving UPPPPPPP");
        }

        public void MoveConnectedDeskDown()
        {
            StopMovingConnectedDeskUp();
            if (!IsConnected || ConnectedDesk.Mode == Mode.Sit) return;

            UpdateModeIfChanged(Mode.MovingDown);
            _sendCommandDownTimer.Start();
            Console.WriteLine("Started moving DOWNNNNNN");
        }

        public void StopMovingConnectedDeskUp()
        {
            _sendCommandUpTimer?.Stop();
            UpdateModeIfChanged(DetermiteCurrentMode());
        }
        public void StopMovingConnectedDeskDown()
        {
            _sendCommandDownTimer?.Stop();
            UpdateModeIfChanged(DetermiteCurrentMode());
        }

        private void UpdateModeIfChanged(Mode mode)
        {
            if (ConnectedDesk.Mode == mode) return;

            Console.WriteLine($"Updated mode from {ConnectedDesk.Mode} to {mode}");
            ConnectedDesk.Mode = mode;
            OnModeChanged?.Invoke();
        }

        private Mode DetermiteCurrentMode()
        {
            Mode mode = _currentHeight <= (_userSettingsService.HeightInStayMode + _userSettingsService.HeightInSitMode) / 2f
                ? Mode.Sit : Mode.Stay;
            return mode;
        }

        private void SetUpTimers()
        {
            _sendCommandUpTimer = new BasicTimer(
                () => Task.Run(SendCommandToMoveUpIfRequired), SendCommandInterval, TimerPriority.HighPriority);
            _sendCommandDownTimer = new BasicTimer(
                () => Task.Run(SendCommandToMoveDownIfRequired), SendCommandInterval, TimerPriority.HighPriority);
        }

        private async Task SendCommandToMoveUpIfRequired()
        {
            if (_currentHeight >= _userSettingsService.HeightInStayMode)
            {
                StopMovingConnectedDeskUp();
                return;
            }

            bool success = await SendCommandToMoveUp();
            if (!success)
            {
                StopMovingConnectedDeskUp();
            }
        }

        private async Task SendCommandToMoveDownIfRequired()
        {
            if (_currentHeight <= _userSettingsService.HeightInSitMode)
            {
                StopMovingConnectedDeskDown();
                return;
            }

            bool success = await SendCommandToMoveDown();
            if (!success)
            {
                StopMovingConnectedDeskDown();
            }
        }

        private async Task<bool> SendCommandToMoveDown()
        {
            var writer = new DataWriter();
            writer.WriteBytes(MoveTableDownCommand);

            GattCommunicationStatus result = await _connectedDeskData.moveDeskCharacteristic.WriteValueAsync(writer.DetachBuffer());
            if (result != GattCommunicationStatus.Success)
            {
                Console.WriteLine("Failed to move down");
                return false;
            }

            return true;
        }

        private async Task<bool> SendCommandToMoveUp()
        {
            var writer = new DataWriter();
            writer.WriteBytes(MoveTabeUpCommand);

            GattCommunicationStatus result = await _connectedDeskData.moveDeskCharacteristic.WriteValueAsync(writer.DetachBuffer());
            if (result != GattCommunicationStatus.Success)
            {
                Console.WriteLine("Failed to move up");
                return false;
            }

            return true;
        }

        #endregion
    }
}
