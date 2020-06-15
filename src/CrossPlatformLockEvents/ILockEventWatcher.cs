using System;

namespace CrossPlatformLockEvents
{
    /// <summary>
    /// Generic interface for all types of watchers for lock events.
    /// </summary>
    internal interface ILockEventWatcher : IDisposable
    {
        void Initialize();
    }
}