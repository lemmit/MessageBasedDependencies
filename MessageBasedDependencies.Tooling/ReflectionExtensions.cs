using System;
using System.Linq;

namespace MessageBasedDependencies.Tooling
{
    public static class ReflectionExtensions
    {
        public static Type[] GetAllMessageTypesHandledBySubscriber(this object @this, Type subscriberBaseInterface)
        {
            return @this.GetType()
                .GetInterfaces()
                .Where(iface => iface.IsGenericType
                                && iface.GetInterfaces().Contains(subscriberBaseInterface))
                .SelectMany(isubscriber => isubscriber.GenericTypeArguments)
                .ToArray();
        }
    }
}
