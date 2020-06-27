using NDesk.DBus;

namespace CrossPlatformLockEvents.DBus
{
    internal interface IDBusFactory
    {
        Connection GetConnection();
    }
}