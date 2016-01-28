using System;
using System.Collections.Generic;
using System.Linq;
using MessageBasedDependencies.Tooling;

namespace MessageBasedDependencies.Base.DeliveryStrategies
{
    public class RandomLoadBalanceDeliveryStrategy : IMessageDeliveryStrategy
    {
        public IEnumerable<ISubscriber> FilterSubscribers(IEnumerable<ISubscriber> subscribers)
        {
            return subscribers.Any() ? 
                    new List<ISubscriber>() {subscribers.RandomElement()} 
                    : new List<ISubscriber>();
            
        }
    }
}
