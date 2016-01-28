using MessageBasedDependencies.Base.DeliveryStrategies;

namespace MessageBasedDependencies.Base.Objects
{
    public class GenericPublisherObject : 
        BaseObject
    {
        public void Publish<T>(T message, string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                Bus.Publish(message);
            }
            else
            {
                Bus.Publish(message, new DeliveryByPathStrategy<T>(path));
            }
        }
    }
}
