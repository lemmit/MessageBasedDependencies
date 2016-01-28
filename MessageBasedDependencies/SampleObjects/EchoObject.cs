using MessageBasedDependencies.Base;
using MessageBasedDependencies.Base.Messages;
using MessageBasedDependencies.Messages;

namespace MessageBasedDependencies.SampleObjects
{
    public class EchoObject : BaseObject,
        IPublisher<PongMessage>,
        IPublisher<LogMessage>,
        ISubscriber<PingMessage>
        
    {
        protected override void OnCreate()
        {
            this.Publish(new LogMessage($"[{ObjectId}] created"));
        }

        public void Receive(PingMessage message)
        {
            this.Publish(new LogMessage($"[{ObjectId}] Ping message!"));
            //Thread.Sleep(20000);
            this.Answer(message, new PongMessage());
        }
    }
}
