using NDesk.DBus;

namespace CrossPlatformLockEvents.DBus
{
    /// <summary>
    /// Uses a system bus.
    /// </summary>
    abstract class AbstractDBusSystemBusEventWatcher : AbstractDBusEventWatcher
    {
        protected override void InitializeDBus()
        {
            _bus = Bus.System;
        }
    }
}