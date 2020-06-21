using System;

namespace CrossPlatformLockEvents
{
    /// <summary>
    ///     Generic interface for all types of watchers for lock events.
    /// </summary>
    internal interface ILockEventWatcher : IDisposable
    {
        /// <summary>
        ///     Initializes the watcher and starts watching for the lock event.
        /// </summary>
        void StartWatching();

        /// <summary>
        /// </summary>
        event EventHandler LockEventObserved;
    }
}