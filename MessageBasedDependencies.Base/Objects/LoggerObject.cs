using System;
using MessageBasedDependencies.Base.Messages;

namespace MessageBasedDependencies.Base.Objects
{
    public class LoggerObject : BaseObject,
        ISubscriber<LogMessage>
    {
        public void Receive(LogMessage message)
        {
            Console.WriteLine($"[LOG:{Path}] {message.Message}");
        }
    }
}
