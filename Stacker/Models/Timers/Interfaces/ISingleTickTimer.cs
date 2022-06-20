using System;

namespace Stacker.Models.Timers
{
    public interface ISingleTickTimer : ITimer
    {
        public void UpdateInterval(TimeSpan interval);
        public void UpdateTick(Action tick);
    }
}
