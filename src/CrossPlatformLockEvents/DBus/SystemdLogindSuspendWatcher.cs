using System;
using NDesk.DBus;

namespace CrossPlatformLockEvents.DBus
{
    internal class SystemdLogindSuspendWatcher : AbstractDBusLockEventWatcher
    {
        public SystemdLogindSuspendWatcher() : this(new SystemDBusFactory())
        {
        }

        public SystemdLogindSuspendWatcher(IDBusFactory dBusFactory) : base(dBusFactory)
        {
        }
        
        protected override void InitializeImpl()
        {
            throw new NotImplementedException();
        }

        protected override void RunImpl()
        {
            throw new NotImplementedException();
            //Syscall.close()
        }

        protected override void ShutdownImpl()
        {
            throw new NotImplementedException();
        }
    }
}
