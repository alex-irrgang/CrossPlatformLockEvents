using System;
using NDesk.DBus;

namespace CrossPlatformLockEvents.DBus
{
    internal class SystemdLogindSuspendWatcher : AbstractDBusSystemBusEventWatcher
    {
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