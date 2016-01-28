using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MessageBasedDependencies.Tooling;

namespace MessageBasedDependencies.Base.DeliveryStrategies
{
    public class DeliveryByPathStrategy<TP> : IMessageDeliveryStrategy
    {
        private readonly string _path;
        public DeliveryByPathStrategy(string path)
        {
            _path = path;
        }
        
        public IEnumerable<ISubscriber> FilterSubscribers(IEnumerable<ISubscriber> subscribers)
        {
            var subscriberSpecifiedByPath = subscribers
                .FirstOrDefault(subscriber => subscriber.Path == _path);
            if (subscriberSpecifiedByPath == null)
            {
                Debug.WriteLine("Subscriber with given path has not been found.");
                return new List<ISubscriber>();
            }
            Debug.WriteLine(
                $"Publish message to {subscriberSpecifiedByPath.GetObjectId()}" +
                $"specified by path {_path}"
            );
            return new List<ISubscriber>() { subscriberSpecifiedByPath };
        }
    }
}
