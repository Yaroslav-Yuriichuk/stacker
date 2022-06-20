using System;
using System.Windows.Threading;

namespace Stacker.Models.Timers
{
    public class BasicTimer : ITimer
    {
        public TimeSpan TimeAfterStart => _isActive ? DateTime.Now - _timeIntervalStarted : TimeSpan.Zero;
        public TimeSpan TimeBeforeTick => _isActive ? _interval  - (DateTime.Now - _timeIntervalStarted) : TimeSpan.Zero;

        private DispatcherTimer _timer;
        private Action _tick;
        private TimeSpan _interval;

        private bool _isActive;
        private DateTime _timeIntervalStarted;

        #region Methods

        public BasicTimer(Action tick, TimeSpan interval, TimerPriority priority = TimerPriority.Normal)
        {
            _tick = tick;
            _interval = interval;

            DispatcherPriority dispatcherPriority = (priority) switch
            {
                TimerPriority.Normal => DispatcherPriority.Normal,
                TimerPriority.HighPriority => DispatcherPriority.Send,
                TimerPriority.LowPriority => DispatcherPriority.Background
            };

            _timer = new DispatcherTimer(dispatcherPriority)
            {
                Interval = interval,
            };
            _timer.Tick += InvokeOnTick;
        }

        ~BasicTimer()
        {
            _timer.Tick -= InvokeOnTick;
        }

        public void Start()
        {
            _timer.Start();
            _isActive = true;
            _timeIntervalStarted = DateTime.Now;
        }

        public void Stop()
        {
            _timer.Stop();
            _isActive = false;
        }

        private void InvokeOnTick(object sender, EventArgs e)
        {
            _tick?.Invoke();
        }

        #endregion
    }

    public enum TimerPriority
    {
        Normal,
        LowPriority,
        HighPriority
    }
}
