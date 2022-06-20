using System;

namespace Stacker.Services
{
    public interface IComputerModeMonitoringService
    {
        public event Action OnSleepModeEntered;
        public event Action OnSleepModeExited;
    }
}
