using System;
using System.Windows.Threading;

namespace Stacker.Models.Timers
{
    public class MultipleIntervalsSingleTickTimer : IMultipleIntervalsSingleTickTimer
    {
        public TimeSpan TimeAfterStart => _isActive ? DateTime.Now - _timeIntervalStarted : TimeSpan.Zero;
        public TimeSpan TimeBeforeTick => _isActive ? _intervals[_currentInterval] - (DateTime.Now - _timeIntervalStarted) : TimeSpan.Zero;

        private DispatcherTimer _timer;
        private Action[] _ticks;
        private TimeSpan[] _intervals;

        private bool _isActive;
        private int _currentInterval;
        private DateTime _timeIntervalStarted;

        #region Methods

        public MultipleIntervalsSingleTickTimer(Action[] ticks, TimeSpan[] intervals)
        {
            _ticks = ticks;
            _intervals = intervals;

            _timer = new DispatcherTimer()
            {
                Interval = intervals[_currentInterval],
            };
            _timer.Tick += InvokeOnTick;
        }

        public void Start()
        {
            _timer.Start();
            _isActive = true;
            _timeIntervalStarted = DateTime.Now;
        }

        public void StartFromIndex(int index)
        {
            _currentInterval = index;
            _timer.Interval = _intervals[_currentInterval];
            Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _isActive = false;
        }

        public void UpdateIntervalAtIndex(TimeSpan interval, int index = 0)
        {
            if (!_isActive || _currentInterval != index)
            {
                _intervals[index] = interval;
                return;
            }

            _timer.Stop();

            double elapsedTimeInMilliseconds =
                        (DateTime.Now - _timeIntervalStarted).TotalMilliseconds;

            int remainingTimeInMilliseconds = (int)interval.TotalMilliseconds - (int)elapsedTimeInMilliseconds;

            if (remainingTimeInMilliseconds <= 0)
            {
                _ticks[_currentInterval]?.Invoke();
                _currentInterval = (_currentInterval + 1) % _ticks.Length;
                _isActive = false;
                return;
            }

            _timer.Interval = new TimeSpan(0, 0, 0, 0, remainingTimeInMilliseconds);
            _timer.Start();
        }

        public void UpdateTickAtIndex(Action tick, int index = 0)
        {
            _ticks[index] = tick;
        }

        private void InvokeOnTick(object sender, EventArgs e)
        {
            _ticks[_currentInterval]?.Invoke();
            Stop();
            _currentInterval = (_currentInterval + 1) %_ticks.Length;
        }

        #endregion
    }
}
