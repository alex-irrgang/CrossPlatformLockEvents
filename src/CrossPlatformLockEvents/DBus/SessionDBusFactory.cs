using NDesk.DBus;

namespace CrossPlatformLockEvents.DBus
{
    class SessionDBusFactory : IDBusFactory
    {
        public Connection GetConnection()
        {
            return Bus.Session;
        }
    }
}