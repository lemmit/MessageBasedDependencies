using System.Threading.Tasks;
using MessageBasedDependencies.Base;
using MessageBasedDependencies.Base.DeliveryStrategies;
using MessageBasedDependencies.Base.Messages;
using MessageBasedDependencies.Messages;

namespace MessageBasedDependencies.SampleObjects
{
    public class AskAnswerPingObject :
        BaseObject,
        ISubscriber<PongMessage>,
        IPublisher<PingMessage>,
        IPublisher<LogMessage>
    {
        public async Task Ping(string path = null)
        {
            this.Publish(new LogMessage($"[{ObjectId}] created!"));
            await this.Ask<PingMessage, PongMessage, AskAnswerPingObject>(
                    new PingMessage(), new DeliveryByPathStrategy<PingMessage>(path)
                );
            this.Publish(new LogMessage($"[{ObjectId}] Someone has answered to ping!"));
        }
        
        public void Receive(PongMessage message)
        {
           this.Publish(new LogMessage($"[{ObjectId}] received a pong message!"));
        }
    }
}
