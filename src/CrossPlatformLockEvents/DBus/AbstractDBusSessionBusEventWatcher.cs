using NDesk.DBus;

namespace CrossPlatformLockEvents.DBus
{
    /// <summary>
    ///     Uses a session bus.
    /// </summary>
    internal abstract class AbstractDBusSessionBusEventWatcher : AbstractDBusEventWatcher
    {
        protected override void InitializeBusImpl()
        {
            DBus = Bus.Session;
        }
    }
}