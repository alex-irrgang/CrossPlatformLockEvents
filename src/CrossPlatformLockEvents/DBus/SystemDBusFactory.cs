using NDesk.DBus;

namespace CrossPlatformLockEvents.DBus
{
    internal class SystemDBusFactory : IDBusFactory
    {
        public Connection GetConnection()
        {
            return Bus.System;
        }
    }
}