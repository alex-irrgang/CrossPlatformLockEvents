using System;

namespace CrossPlatformLockEvents
{
    internal class LockEventArgs : EventArgs
    {
        public LockEventType ObservedEvent { get; private set; }

        public LockEventArgs(LockEventType observedEvent)
        {
            ObservedEvent = observedEvent;
        }
    }
}