using System;

namespace Stacker.Interfaces
{
    public interface IDesk : IDevice
    {
        public event Action<ConnectionState> OnConnectionStateChanged;
        public ConnectionState ConnectionState { get; set; }
    }

    public enum ConnectionState
    {
        Connected,
        Disconnected,
        Connecting
    }
}
