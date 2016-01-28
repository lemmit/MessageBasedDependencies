using System.Collections.Generic;

namespace MessageBasedDependencies.Base
{
    public interface IMessageDeliveryStrategy
    {
        IEnumerable<ISubscriber> FilterSubscribers(IEnumerable<ISubscriber> subscribers);
    }
}
