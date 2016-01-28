using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MessageBasedDependencies.Base.DeliveryStrategies;
using MessageBasedDependencies.Tooling;

namespace MessageBasedDependencies.Base.Buses
{
    public class InMemoryBus : IBus
    {
        private readonly MessagesWaitingForCompletion _messages =
            new MessagesWaitingForCompletion();

        private readonly SubscribersByMessageType _subscribers
            = new SubscribersByMessageType();
        public readonly IMessageDeliveryStrategy DefaultDeliveryStrategy;

        private readonly object _sync = new object();
        private readonly bool _async;
        public InMemoryBus(bool async = true, IMessageDeliveryStrategy defaultDeliveryStrategy = null)
        {
            _async = async;
            DefaultDeliveryStrategy = defaultDeliveryStrategy ?? new FanOutDeliveryStrategy();
        }
       
        public Task<TR> Ask<TP, TR, TS>(TS publisher, TP message, IMessageDeliveryStrategy deliveryStrategy)
            where TS : IPublisher<TP>, ISubscriber<TR>
        {
            deliveryStrategy = deliveryStrategy ?? DefaultDeliveryStrategy;
            Debug.WriteLine($"Ask: {message.GetObjectId()}");
            var future = new TaskCompletionSource<TR>();
            _messages[message] = future;
            Publish(message, deliveryStrategy);
            return future.Task;
        }

        public void Answer<TP, TR>(TP request, TR response)
        {
            
                Debug.WriteLine($"Answer: {request.GetObjectId()} => {response.GetObjectId()}");
                ITaskCompletionSource taskCompletionSource;
                if (_messages.TryGetValue(request, out taskCompletionSource))
                {
                    Debug.WriteLine($"AskCompletion found. Sending answer.");
                    ((TaskCompletionSource<TR>) taskCompletionSource).SetResult(response);
                    ITaskCompletionSource sr;
                    _messages.TryRemove(request, out sr);
                }
                else
                {
                    Debug.WriteLine($"AskCompletion not found. Publishing message.");
                    Publish(response, DefaultDeliveryStrategy);
                }
            
        }

        public void Publish<TP>(TP message, IMessageDeliveryStrategy deliveryStrategy)
        {
            deliveryStrategy = deliveryStrategy ?? DefaultDeliveryStrategy;

            var sendTo = deliveryStrategy.FilterSubscribers(GetSubscribers<TP>());
            sendTo.ToList().ForEach(subscriber =>
            {
                try
                {
                    Debug.WriteLine(
                        $"Publish {message.GetObjectId()} " +
                        $"to {subscriber.GetObjectId()}"
                        );
                    if (_async)
                    {
                        Task.Run(() =>
                        {
                            ((ISubscriber<TP>) subscriber).Receive(message);
                        });
                    }
                    else ((ISubscriber<TP>) subscriber).Receive(message);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            });
        }

        public void Subscribe<TS>(ISubscriber<TS> subscriber)
        {
            lock (_sync)
            {
                var subscribedToMessage = GetSubscribers<TS>();
                subscribedToMessage.Add(subscriber);
                _subscribers[typeof (TS)] = subscribedToMessage;
            }
        }

        private GroupOfSubscribers GetSubscribers<T>()
        {
            GroupOfSubscribers subscribedToMessageType;
            //Please ignore R# warning. 
            if (_subscribers.TryGetValue(typeof (T), out subscribedToMessageType))
            {
                return new GroupOfSubscribers(subscribedToMessageType);
            }
            return new GroupOfSubscribers();
        }

        #region Private types definitions

        private interface ITaskCompletionSource
        {
        }

        private class TaskCompletionSource<TP> : System.Threading.Tasks.TaskCompletionSource<TP>, ITaskCompletionSource
        {
        }

        private class GroupOfSubscribers : ConcurrentBag<ISubscriber>
        {
            public GroupOfSubscribers()
            {
            }

            public GroupOfSubscribers(IEnumerable<ISubscriber> collection) : base(collection)
            {
            }
        }

        private class SubscribersByMessageType :
            ConcurrentDictionary<Type, GroupOfSubscribers>
        {
        }

        private class MessagesWaitingForCompletion :
            ConcurrentDictionary<object, ITaskCompletionSource>
        {
        }

        #endregion
    }
}