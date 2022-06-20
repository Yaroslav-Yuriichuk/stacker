using System;

namespace Stacker.Services
{
    public interface IAutomationService
    {
        public bool IsEnabled { get; }

        public void Enable();
        public void Disable();

        public void UpdateIntervalForStayMode(TimeSpan interval);
        public void UpdateIntervalForSitMode(TimeSpan interval);
    }
}
