using Stacker.Interfaces;
using System.Threading.Tasks;

namespace Stacker.Models
{
    public class ConnectedDesk : IConnectedDesk
    {
        public string Id => _desk.Id;
        public string Name => _desk.Name;
        public Mode Mode { get; set; } = Mode.Undefined;
        public int CurrentHeight { get; set; }

        private readonly IDesk _desk;

        #region Methods

        public ConnectedDesk(IDesk desk)
        {
            _desk = desk;
        }

        public async Task Disconnect()
        {
            _desk.ConnectionState = ConnectionState.Disconnected;
        }

        #endregion
    }
}
