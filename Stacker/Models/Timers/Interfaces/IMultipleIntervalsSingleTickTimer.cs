using System;

namespace Stacker.Models.Timers
{
    public interface IMultipleIntervalsSingleTickTimer : ITimer
    {
        public void StartFromIndex(int index);

        public void UpdateIntervalAtIndex(TimeSpan interval, int index = 0);
        public void UpdateTickAtIndex(Action tick, int index = 0);
    }
}
