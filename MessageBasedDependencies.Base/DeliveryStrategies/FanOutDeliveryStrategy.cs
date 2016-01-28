using System;
using System.Collections.Generic;

namespace MessageBasedDependencies.Base.DeliveryStrategies
{
    public class FanOutDeliveryStrategy : IMessageDeliveryStrategy
    {
        public IEnumerable<ISubscriber> FilterSubscribers(IEnumerable<ISubscriber> subscribers)
        {
            return subscribers;
        }
    }
}
