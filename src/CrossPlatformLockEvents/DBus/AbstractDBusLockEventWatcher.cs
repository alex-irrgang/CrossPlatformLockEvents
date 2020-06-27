using System;
using System.Threading;
using NDesk.DBus;

namespace CrossPlatformLockEvents.DBus
{
    internal abstract class AbstractDBusLockEventWatcher : ILockEventWatcher
    {
        private readonly ManualResetEvent _dbusStopEvent;
        private readonly Thread _dbusThread;
        private bool _started;
        private bool _disposed;
        protected readonly Connection DBusConnection;

        /// <summary>
        ///     How long to wait in the main loop for the shutdown event.
        /// </summary>
        protected int LoopTimeout = 100;

        protected AbstractDBusLockEventWatcher(IDBusFactory dbusFactory)
        {
            DBusConnection = dbusFactory.GetConnection();
            _dbusThread = new Thread(RunWrapper);
            _dbusStopEvent = new ManualResetEvent(false);
        }

        public void StartWatching()
        {
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
                    DBusConnection.Iterate();
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
                DBusConnection.Close();
            }

            _disposed = true;
        }

        ~AbstractDBusLockEventWatcher()
        {
            Dispose(false);
        }
    }
}
