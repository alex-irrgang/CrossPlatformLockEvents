using System;
using System.Windows.Forms;
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
        private LocKEventManager _lockEventManager;

        private IPluginHost _pluginHost;
        public bool LockOnScreensaver { get; private set; }
        public bool LockOnSuspend { get; private set; }

        public override bool Initialize(IPluginHost host)
        {
            if (null == host) return false;

            _pluginHost = host;

            LockOnScreensaver = _pluginHost.CustomConfig.GetBool(LockOnScreensaverConfigKey, true);
            LockOnSuspend = _pluginHost.CustomConfig.GetBool(LockOnSuspendConfigKey, true);

            _lockEventManager = new LocKEventManager(_pluginHost, () => LockOnScreensaver, () => LockOnSuspend);
            _lockEventManager.Initialize();

            return true;
        }

        public override void Terminate()
        {
            _lockEventManager.Terminate();
        }

        public override ToolStripMenuItem GetMenuItem(PluginMenuType menuType)
        {
            if (menuType != PluginMenuType.Main) return null;

            var mainLevel = new ToolStripMenuItem {Text = "CrossPlatformLockEvents Settings"};

            var screensaverLock = new ToolStripMenuItem
            {
                Text = "Lock on Screensaver",
                CheckOnClick = true,
                Checked = LockOnScreensaver
            };
            screensaverLock.CheckedChanged += ScreensaverLockOnCheckedChanged;

            var suspendLock = new ToolStripMenuItem
            {
                Text = "Lock on Suspend",
                CheckOnClick = true,
                Checked = LockOnSuspend
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

            LockOnSuspend = suspendLockMenuItem.Checked;
            _pluginHost.CustomConfig.SetBool(LockOnSuspendConfigKey, LockOnSuspend);
        }

        private void ScreensaverLockOnCheckedChanged(object sender, EventArgs e)
        {
            var screensaverLockMenuItem = sender as ToolStripMenuItem;
            if (screensaverLockMenuItem == null) return;

            LockOnScreensaver = screensaverLockMenuItem.Checked;
            _pluginHost.CustomConfig.SetBool(LockOnScreensaverConfigKey, LockOnScreensaver);
        }
    }
}