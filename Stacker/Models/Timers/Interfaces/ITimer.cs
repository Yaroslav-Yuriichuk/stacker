using System;

namespace Stacker.Models.Timers
{
    public interface ITimer
    {
        public TimeSpan TimeAfterStart { get; }
        public TimeSpan TimeBeforeTick { get; }

        public void Start();
        public void Stop();
    }
}
