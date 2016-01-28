using MessageBasedDependencies.Base;
using MessageBasedDependencies.Base.DeliveryStrategies;
using MessageBasedDependencies.Base.Messages;
using MessageBasedDependencies.Messages;

namespace MessageBasedDependencies.SampleObjects
{
    public class SendingLogByPathObject : BaseObject,
        IPublisher<LogMessage>
    {
        public void SendLogByPath(string path)
        {
            this.PublishByPath(
                new LogMessage($"[{ObjectId}] log using {path} by path"), 
                path
            );
        }
    }
}
