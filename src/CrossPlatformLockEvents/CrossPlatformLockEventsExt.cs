using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using CrossPlatformLockEvents.DBus;
using KeePass.Plugins;

namespace CrossPlatformLockEvents
{
    /// <summary>
    ///     CrossPlatformLockEvents enables cross-platform auto locking mechanisms.
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public sealed class CrossPlatformLockEventsExt : Plugin
    {
        private const string LockOnScreensaverConfigKey =
            "io.github.alex-irrgang.CrossPlatformLockEvents.LockOnScreensaver";

        private const string LockOnSuspendConfigKey = "io.github.alex-irrgang.CrossPlatformLockEvents.LockOnSuspend";
        
        private IPluginHost _pluginHost;
        private readonly List<ILockEventWatcher> _lockEventWatchers = new List<ILockEventWatcher>();
        private readonly Dictionary<LockEventType, bool> _lockEventTypeEnabled = new Dictionary<LockEventType, bool>();

        public override bool Initialize(IPluginHost host)
        {
            if (null == host) return false;

            _pluginHost = host;

            _lockEventTypeEnabled.Add(LockEventType.Screensaver,
                _pluginHost.CustomConfig.GetBool(LockOnScreensaverConfigKey, true));
            _lockEventTypeEnabled.Add(LockEventType.Suspend,
                _pluginHost.CustomConfig.GetBool(LockOnSuspendConfigKey, true));

            InitializeLockEventWatchers();

            return true;
        }

        private void InitializeLockEventWatchers()
        {
            _lockEventWatchers.Add(new SystemdLogindSuspendWatcher());

            foreach (var lockEventWatcher in _lockEventWatchers)
            {
                lockEventWatcher.LockEventObserved += LockEventWatcherOnLockEventObserved;
                lockEventWatcher.StartWatching();
            }
        }

        private void LockEventWatcherOnLockEventObserved(object sender, LockEventArgs e)
        {
            if (_lockEventTypeEnabled[e.ObservedEvent]) _pluginHost.MainWindow.LockAllDocuments();
        }

        public override void Terminate()
        {
            try
            {
                foreach (var lockEventWatcher in _lockEventWatchers) lockEventWatcher.Dispose();
            }
            catch (Exception e)
            {
                // TODO: logging
            }
        }

        public override ToolStripMenuItem GetMenuItem(PluginMenuType menuType)
        {
            if (menuType != PluginMenuType.Main) return null;

            var mainLevel = new ToolStripMenuItem {Text = "CrossPlatformLockEvents Settings"};

            var screensaverLock = new ToolStripMenuItem
            {
                Text = "Lock on Screensaver",
                CheckOnClick = true,
                Checked = _lockEventTypeEnabled[LockEventType.Screensaver]
            };
            screensaverLock.CheckedChanged += ScreensaverLockOnCheckedChanged;

            var suspendLock = new ToolStripMenuItem
            {
                Text = "Lock on Suspend",
                CheckOnClick = true,
                Checked = _lockEventTypeEnabled[LockEventType.Suspend]
            };
            suspendLock.CheckedChanged += SuspendLockOnCheckedChanged;

            mainLevel.DropDownItems.Add(screensaverLock);
            mainLevel.DropDownItems.Add(suspendLock);

            return mainLevel;
        }

        private void SuspendLockOnCheckedChanged(object sender, EventArgs e)
        {
            var suspendLockMenuItem = sender as ToolStripMenuItem;
            if (suspendLockMenuItem == null) return;

            _lockEventTypeEnabled[LockEventType.Suspend] = suspendLockMenuItem.Checked;
            _pluginHost.CustomConfig.SetBool(LockOnSuspendConfigKey, suspendLockMenuItem.Checked);
        }

        private void ScreensaverLockOnCheckedChanged(object sender, EventArgs e)
        {
            var screensaverLockMenuItem = sender as ToolStripMenuItem;
            if (screensaverLockMenuItem == null) return;

            _lockEventTypeEnabled[LockEventType.Screensaver] = screensaverLockMenuItem.Checked;
            _pluginHost.CustomConfig.SetBool(LockOnScreensaverConfigKey, screensaverLockMenuItem.Checked);
        }
    }
}