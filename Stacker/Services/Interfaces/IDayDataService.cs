using System;

namespace Stacker.Services
{
    public interface IDayDataService
    {
        public event Action<DayData> OnTick;
        public DayData DayData { get; }
    }

    [Serializable]
    public struct DayData
    {
        public DateTime Date;
        public TimeSpan TimeSpentInStayMode;
        public TimeSpan TimeSpentInSitMode;


        public DayData(DateTime date, TimeSpan timeSpentInStayMode = new TimeSpan(), TimeSpan timeSpentInSitMode = new TimeSpan())
        {
            Date = date;
            TimeSpentInStayMode = timeSpentInStayMode;
            TimeSpentInSitMode = timeSpentInSitMode;
        }
    }
}
