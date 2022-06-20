using System;

namespace Stacker.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        public TimeSpan IntervalForStayMode => new TimeSpan(0, Properties.Settings.Default.IntervalForStayMode, 0);
        public TimeSpan IntervalForSitMode => new TimeSpan(0, Properties.Settings.Default.IntervalForSitMode, 0);
        public TimeSpan MoveDeskAfterNotificationInterval => new TimeSpan(0, Properties.Settings.Default.MoveDeskAfterNotificationInterval, 0);
        public TimeSpan SnoozeInterval => new TimeSpan(0, Properties.Settings.Default.SnoozeInterval, 0);

        public int HeightInStayMode => Properties.Settings.Default.HeightInStayMode;
        public int HeightInSitMode => Properties.Settings.Default.HeightInSitMode;


        public void UpdateIntervalForSitMode(TimeSpan interval)
        {
            Properties.Settings.Default.IntervalForSitMode = (int)interval.TotalMinutes;
            Properties.Settings.Default.Save();
        }

        public void UpdateIntervalForStayMode(TimeSpan interval)
        {
            Properties.Settings.Default.IntervalForStayMode = (int)interval.TotalMinutes;
            Properties.Settings.Default.Save();
        }

        public void UpdateMoveDeskAfterNotificationInterval(TimeSpan interval)
        {
            Properties.Settings.Default.MoveDeskAfterNotificationInterval = (int)interval.TotalMinutes;
            Properties.Settings.Default.Save();
        }

        public void UpdateSnoozeInterval(TimeSpan interval)
        {
            Properties.Settings.Default.SnoozeInterval = (int)interval.TotalMinutes;
            Properties.Settings.Default.Save();
        }

        public void UpdateHeightInSitMode(int height)
        {
            Properties.Settings.Default.HeightInSitMode = height;
            Properties.Settings.Default.Save();
        }

        public void UpdateHeightInStayMode(int height)
        {
            Properties.Settings.Default.HeightInStayMode = height;
            Properties.Settings.Default.Save();
        }
    }
}
