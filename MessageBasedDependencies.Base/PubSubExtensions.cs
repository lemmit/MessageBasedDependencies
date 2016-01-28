using System.Threading.Tasks;
using MessageBasedDependencies.Base.DeliveryStrategies;

namespace MessageBasedDependencies.Base
{
    public static class PubSubExtensions
    {
        public static Task<TR> Ask<TP, TR, TS>(this TS @this, TP message, IMessageDeliveryStrategy deliveryStrategy = null)
            where TS : BaseObject, IPublisher<TP>, ISubscriber<TR>
        {
            return @this.Bus.Ask<TP, TR, TS>(@this, message, deliveryStrategy);
        }

        public static void Answer<TP, TR, TS>(this TS @this, TP request, TR response)
            where TS : BaseObject, ISubscriber<TP>, IPublisher<TR>
        {
            @this.Bus.Answer(request, response);
        }

        public static void Publish<T, TR>(this TR @this, T message, IMessageDeliveryStrategy deliveryStrategy = null)
            where TR : BaseObject, IPublisher<T>
        {
            @this.Bus.Publish(message, deliveryStrategy);
        }

        public static void PublishByPath<T, TR>(this TR @this, T message, string path)
            where TR : BaseObject, IPublisher<T>
        {
            @this.Bus.Publish(message, new DeliveryByPathStrategy<T>(path));
        }

    }
}
