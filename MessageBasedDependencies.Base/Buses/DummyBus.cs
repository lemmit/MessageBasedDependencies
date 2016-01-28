using System.Diagnostics;
using System.Threading.Tasks;

namespace MessageBasedDependencies.Base.Buses
{
    internal class DummyBus : IBus
    {
        public void Answer<TP, TR>(TP request, TR response)
        {
            Debug.WriteLine($"DummyBus.Answer({request}, {response})");
        }

        public Task<TR> Ask<TP, TR, TS>(TS publisher, TP message, IMessageDeliveryStrategy deliveryStrategy) 
            where TS : IPublisher<TP>, ISubscriber<TR>
        {
            Debug.WriteLine($"DummyBus.Ask({publisher}, {message}, ... )");
            return Task.Run(() => default(TR));
        }

        public void Publish<TP>(TP message, IMessageDeliveryStrategy deliveryStrategy)
        {
            Debug.WriteLine($"DummyBus.Publish({message}, ...)");
        }

        public void Subscribe<TS>(ISubscriber<TS> subscriber)
        {
            Debug.WriteLine($"DummyBus.Subscribe({subscriber})");
        }
    }
}
