using System.Threading.Tasks;

namespace MessageBasedDependencies.Base
{
    public interface IBus
    {
        void Publish<TP>(TP message, IMessageDeliveryStrategy deliveryStrategy = null);
        Task<TR> Ask<TP, TR, TS>(TS publisher, TP message, IMessageDeliveryStrategy deliveryStrategy= null)
            where TS : IPublisher<TP>, ISubscriber<TR>;
        void Answer<TP, TR>(TP request, TR response);
        void Subscribe<TS>(ISubscriber<TS> subscriber);
    }
}
