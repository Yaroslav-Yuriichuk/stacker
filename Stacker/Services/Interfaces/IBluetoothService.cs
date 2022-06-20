using Stacker.Interfaces;
using System;
using System.Threading.Tasks;

namespace Stacker.Services
{
    public interface IBluetoothService
    {
        public bool IsConnected { get; }
        public IConnectedDesk ConnectedDesk { get; }

        public event Action OnDeskConnected;
        public event Action OnDeskDisconnected;

        public event Action OnModeChanged;

        public event Action<IDesk> OnDeskAdded;
        public event Action<IDesk> OnDeskRemoved;
        
        public void StartScanning();
        public void StopScanning();

        public Task Connect(IDesk desk);
        public Task Disconnect();

        public void MoveConnectedDeskUp();
        public void MoveConnectedDeskDown();

        public void StopMovingConnectedDeskUp();
        public void StopMovingConnectedDeskDown();
    }
}
