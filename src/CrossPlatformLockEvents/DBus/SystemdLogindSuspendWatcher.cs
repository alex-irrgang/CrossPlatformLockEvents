using Mono.Unix.Native;

namespace CrossPlatformLockEvents.DBus
{
    internal class SystemdLogindSuspendWatcher : AbstractDBusSystemBusEventWatcher
    {
        protected override void Run()
        {
            throw new System.NotImplementedException();
            //Syscall.close()
        }
    }
}