using System;
using System.Collections.Generic;
using CrossPlatformLockEvents.DBus;

namespace CrossPlatformLockEvents
{
    internal class LocKEventManager
    {
        private readonly Func<bool> _lockOnScreensaver;
        private readonly Func<bool> _lockOnSuspend;
        private readonly List<ILockEventWatcher> _lockEventWatchers;


        public LocKEventManager(Func<bool> lockOnScreensaver, Func<bool> lockOnSuspend)
        {
            _lockOnScreensaver = lockOnScreensaver;
            _lockOnSuspend = lockOnSuspend;

            _lockEventWatchers = new List<ILockEventWatcher>();
        }

        public void Initialize()
        {
            _lockEventWatchers.Add(new SystemdLogindSuspendWatcher());
        }

        public void Terminate()
        {
            foreach (var lockEventWatcher in _lockEventWatchers)
            {
                lockEventWatcher.Dispose();
            }
            
            _lockEventWatchers.Clear();
        }
    }
}