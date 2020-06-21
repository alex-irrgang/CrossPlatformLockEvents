using System;
using System.Collections.Generic;
using CrossPlatformLockEvents.DBus;
using KeePass.Plugins;

namespace CrossPlatformLockEvents
{
    internal class LocKEventManager
    {
        private readonly List<ILockEventWatcher> _lockEventWatchers;
        private readonly Func<bool> _lockOnScreensaver;
        private readonly Func<bool> _lockOnSuspend;
        private readonly IPluginHost _pluginHost;


        public LocKEventManager(IPluginHost pluginHost, Func<bool> lockOnScreensaver, Func<bool> lockOnSuspend)
        {
            _pluginHost = pluginHost;
            _lockOnScreensaver = lockOnScreensaver;
            _lockOnSuspend = lockOnSuspend;

            _lockEventWatchers = new List<ILockEventWatcher>();
        }

        public void Initialize()
        {
            _lockEventWatchers.Add(new SystemdLogindSuspendWatcher());

            foreach (var lockEventWatcher in _lockEventWatchers)
            {
                lockEventWatcher.LockEventObserved += LockEventWatcherOnLockEventObserved;
                lockEventWatcher.StartWatching();
            }
        }

        private void LockEventWatcherOnLockEventObserved(object sender, EventArgs e)
        {
            _pluginHost.MainWindow.LockAllDocuments();
        }

        public void Terminate()
        {
            foreach (var lockEventWatcher in _lockEventWatchers) lockEventWatcher.Dispose();

            _lockEventWatchers.Clear();
        }
    }
}