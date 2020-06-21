using NDesk.DBus;

namespace CrossPlatformLockEvents.DBus
{
    /// <summary>
    ///     Uses a system bus.
    /// </summary>
    internal abstract class AbstractDBusSystemBusEventWatcher : AbstractDBusEventWatcher
    {
        protected override void InitializeBusImpl()
        {
            DBus = Bus.System;
        }
    }
}