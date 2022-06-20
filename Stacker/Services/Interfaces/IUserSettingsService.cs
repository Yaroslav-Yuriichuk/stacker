using System;

namespace Stacker.Services
{
    public interface IUserSettingsService
    {
        public TimeSpan IntervalForStayMode { get; }
        public TimeSpan IntervalForSitMode { get; }
        public TimeSpan MoveDeskAfterNotificationInterval { get; }
        public TimeSpan SnoozeInterval { get; }

        public int HeightInStayMode { get; }
        public int HeightInSitMode { get; }

        public void UpdateIntervalForStayMode(TimeSpan interval);
        public void UpdateIntervalForSitMode(TimeSpan interval);
        public void UpdateMoveDeskAfterNotificationInterval(TimeSpan interval);
        public void UpdateSnoozeInterval(TimeSpan interval);

        public void UpdateHeightInStayMode(int height);
        public void UpdateHeightInSitMode(int height);
    }
}
