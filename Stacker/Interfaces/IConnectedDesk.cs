using System.Threading.Tasks;

namespace Stacker.Interfaces
{
    public interface IConnectedDesk : IDevice
    {
        public Mode Mode { get; set; }
        public int CurrentHeight { get; set; }
        public Task Disconnect();
    }

    public enum Mode
    {
        Undefined,
        Sit,
        Stay,
        MovingUp,
        MovingDown
    }
}
