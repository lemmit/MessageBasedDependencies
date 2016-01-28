using System;

namespace MessageBasedDependencies.Base.Objects
{
    public class ActionSubscriberObject<T> : BaseObject,
        ISubscriber<T>
    {
        private readonly Action<T> _onMessage;
        public ActionSubscriberObject(Action<T> onMessage)
        {
            _onMessage = onMessage;
        } 
        public void Receive(T message)
        {
            _onMessage(message);
        }
    }
}
