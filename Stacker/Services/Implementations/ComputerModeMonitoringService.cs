using Microsoft.Win32;
using System;

namespace Stacker.Services
{
    public class ComputerModeMonitoringService : IComputerModeMonitoringService
    {
        public event Action OnSleepModeEntered;
        public event Action OnSleepModeExited;

        public ComputerModeMonitoringService()
        {
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }

        ~ComputerModeMonitoringService()
        {
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.SessionLock:
                    OnSleepModeEntered?.Invoke();
                    break;
                case SessionSwitchReason.SessionUnlock:
                    OnSleepModeExited?.Invoke();
                    break;
            }
        }
    }
}
