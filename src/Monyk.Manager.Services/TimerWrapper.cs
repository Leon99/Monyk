using System;
using System.Threading;

namespace Monyk.Manager.Services
{
    public class TimerFactory
    {
        public ITimer<T> Create<T>(TimeSpan interval, T state, Action<T> timerElapsedHandler)
        {
            return new TimerWrapper<T> {Interval = interval, State = state, Elapsed = timerElapsedHandler};
        }
    }

    public interface ITimer<T>
    {
        TimeSpan Interval { get; set; }
        T State { get; set; }

        void Start();
        void Stop();
    }


    public class TimerWrapper<T> : ITimer<T>, IDisposable
    {
        public Action<T> Elapsed;

        public TimeSpan Interval { get; set; }
        public T State { get; set; }

        private Timer _timer;

        public void Start()
        {
            if (_timer != null)
            {
                Stop();
            }

            _timer = new Timer(state => Elapsed((T) state), State, TimeSpan.Zero, Interval);
        }

        public void Stop()
        {
            _timer.Dispose();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
