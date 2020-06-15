using System;
using System.Threading;
using NDesk.DBus;

namespace CrossPlatformLockEvents.DBus
{
    internal abstract class AbstractDBusEventWatcher : ILockEventWatcher
    {
        protected int LoopTimeout = 100;
        private readonly Thread _dbusThread;
        private readonly ManualResetEvent _dbusStopEvent;
        protected Bus _bus;

        protected AbstractDBusEventWatcher()
        {
            _dbusThread = new Thread(RunWrapper);
            _dbusStopEvent = new ManualResetEvent(false);
        }

        /// <summary>
        /// Implementation will be called cyclically. No bus iteration or thread handling is required.
        /// </summary>
        protected abstract void Run();

        /// <summary>
        /// Handle thread exit.
        /// </summary>
        private void RunWrapper()
        {
            while (!_dbusStopEvent.WaitOne(TimeSpan.FromMilliseconds(LoopTimeout)))
            {
                _bus.Iterate();
                Run();
            }
        }
        
        public void Dispose()
        {
            _dbusStopEvent.Set();
            _dbusThread.Join();
        }

        public void Initialize()
        {
            InitializeDBus();
            _dbusThread.Start();
        }

        /// <summary>
        /// Shall initialize the required D-Bus type, i.e. session bus, or system bus or custom bus.
        /// </summary>
        protected abstract void InitializeDBus();
    }
}