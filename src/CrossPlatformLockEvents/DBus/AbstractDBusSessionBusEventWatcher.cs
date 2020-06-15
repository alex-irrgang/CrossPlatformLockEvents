using NDesk.DBus;

namespace CrossPlatformLockEvents.DBus
{
    /// <summary>
    /// Uses a session bus.
    /// </summary>
    internal abstract class AbstractDBusSessionBusEventWatcher : AbstractDBusEventWatcher
    {
        protected override void InitializeDBus()
        {
            _bus = Bus.Session;
        }
    }
}