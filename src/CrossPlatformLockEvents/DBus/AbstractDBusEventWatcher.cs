using System;
using System.Threading;
using NDesk.DBus;

namespace CrossPlatformLockEvents.DBus
{
    internal abstract class AbstractDBusEventWatcher : ILockEventWatcher
    {
        private readonly ManualResetEvent _dbusStopEvent;
        private readonly Thread _dbusThread;
        private bool _started;
        private bool _disposed;
        protected Bus DBus;

        /// <summary>
        ///     How long to wait in the main loop for the shutdown event.
        /// </summary>
        protected int LoopTimeout = 100;

        protected AbstractDBusEventWatcher()
        {
            _dbusThread = new Thread(RunWrapper);
            _dbusStopEvent = new ManualResetEvent(false);
        }

        public void StartWatching()
        {
            InitializeBusImpl();
            _dbusThread.Start();
        }

        public event EventHandler<LockEventArgs> LockEventObserved;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Call in RunImp() when an event was detected.
        /// </summary>
        protected void OnLockEventObserved(LockEventArgs lockEventArgs)
        {
            LockEventObserved?.Invoke(this, lockEventArgs);
        }

        /// <summary>
        ///     Set this.DBus to the required D-DBus, i.e. system or session bus.
        /// </summary>
        protected abstract void InitializeBusImpl();

        /// <summary>
        ///     Implementation will be called right after starting the thread.
        /// </summary>
        protected abstract void InitializeImpl();

        /// <summary>
        ///     Implementation will be called cyclically. No bus iteration or thread handling is required.
        /// </summary>
        protected abstract void RunImpl();

        /// <summary>
        ///     Implementation will be called right before exiting the thread. This method shall not throw.
        /// </summary>
        protected abstract void ShutdownImpl();

        /// <summary>
        ///     Handle thread exit.
        /// </summary>
        private void RunWrapper()
        {
            try
            {
                InitializeImpl();

                while (!_dbusStopEvent.WaitOne(TimeSpan.FromMilliseconds(LoopTimeout)))
                {
                    DBus.Iterate();
                    RunImpl();
                }                
            }
            catch (Exception e)
            {
                // TODO: Error logging
            }
            finally
            {
                ShutdownImpl();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _dbusStopEvent.Set();
                _dbusThread.Join();
            }

            _disposed = true;
        }

        ~AbstractDBusEventWatcher()
        {
            Dispose(false);
        }
    }
}
