using System;
using System.Windows.Threading;

namespace Stacker.Models.Timers
{
    public class SingleTickTimer : ISingleTickTimer
    {
        public TimeSpan TimeAfterStart => _isActive ? DateTime.Now - _timeIntervalStarted : TimeSpan.Zero;
        public TimeSpan TimeBeforeTick => _isActive ? _interval - (DateTime.Now - _timeIntervalStarted) : TimeSpan.Zero;

        private DispatcherTimer _timer;
        private Action _tick;
        private TimeSpan _interval;

        private bool _isActive;
        private DateTime _timeIntervalStarted;

        #region Methods

        public SingleTickTimer(Action tick, TimeSpan interval)
        {
            _tick = tick;
            _interval = interval;

            _timer = new DispatcherTimer()
            {
                Interval = interval,
            };
            _timer.Tick += InvokeOnTick;
        }

        ~SingleTickTimer()
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

        public void UpdateInterval(TimeSpan interval)
        {
            if (!_isActive)
            {
                _timer.Interval = interval;
                return;
            }

            _timer.Stop();

            double elapsedTimeInMilliseconds =
                        (DateTime.Now - _timeIntervalStarted).TotalMilliseconds;

            int remainingTimeInMilliseconds = (int)interval.TotalMilliseconds - (int)elapsedTimeInMilliseconds;

            if (remainingTimeInMilliseconds <= 0)
            {
                _tick?.Invoke();
                _isActive = false;
                return;
            }

            _timer.Interval = new TimeSpan(0, 0, 0, 0, remainingTimeInMilliseconds);
            _timer.Start();
        }

        public void UpdateTick(Action tick)
        {
            _tick = tick;
        }

        private void InvokeOnTick(object sender, EventArgs e)
        {
            _tick?.Invoke();
            Stop();
        }

        #endregion
    }
}
