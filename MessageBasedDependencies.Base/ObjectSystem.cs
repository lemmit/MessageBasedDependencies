using System;

namespace MessageBasedDependencies.Base
{
    public class ObjectSystem
    {
        private readonly BaseObject.InternalObjectSystem _internalObjectSystem;
        public ObjectSystem(string path, IBus bus)
        {
            _internalObjectSystem = new BaseObject.InternalObjectSystem(path, bus);
        }

        public T Create<T>(Func<T> createObject, string path = null)
            where T : BaseObject
        {
            return _internalObjectSystem.Create(createObject, path);
        }
    }
}
