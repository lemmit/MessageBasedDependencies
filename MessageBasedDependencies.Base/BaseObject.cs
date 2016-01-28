using System;
using System.Diagnostics;
using System.Linq;
using MessageBasedDependencies.Base.Buses;
using MessageBasedDependencies.Tooling;

namespace MessageBasedDependencies.Base
{
    public abstract class BaseObject
    {
        private static readonly IBus DummyBus = new DummyBus();
        private static uint _id = 1;
        private readonly object _sync = new object();
        protected BaseObject()
        {
        }

        protected BaseObject(string path, IBus bus)
        {
            Path = path;
            Bus = bus;
        }

        protected T Create<T>(Func<T> createObject, string path = null)
            where T : BaseObject
        {
            return new InternalObjectSystem(Path, Bus)
                .Create<T>(createObject, path);
        }

        public string ObjectId => GetType().Name + "|" + GetHashCode()%1000;
        public string Path { get; private set; }
        public IBus Bus { get; private set; } = DummyBus;

        protected void InitializeSubscribers()
        {
            var initializeSubscriberMethod = Bus.GetType().GetMethod("Subscribe");
            this.GetAllMessageTypesHandledBySubscriber(typeof (ISubscriber))
                .ToList()
                .ForEach(messageType =>
                    initializeSubscriberMethod.MakeGenericMethod(messageType).Invoke(Bus, new object[] {this})
                );
        }

        #region Overrideable methods [life cycle management]

        protected virtual void OnCreate()
        {
        }

        #endregion

        #region Private methods

        private string GetPath()
        {
            lock (_sync)
            {
                return GetType().Name + "/" + _id++;
            }
        }

        #endregion

        #region Object system factory

        internal class InternalObjectSystem
        {
            public readonly IBus Bus;
            public readonly string Path;

            public InternalObjectSystem(string path, IBus bus)
            {
                Path = path;
                Bus = bus;
            }

            public T Create<T>(Func<T> createObject, string path = null)
                where T : BaseObject
            {
                var obj = createObject();
                obj.Path = Path + "/" + (string.IsNullOrEmpty(path) ? obj.GetPath() : path);
                Debug.WriteLine($"Factory created an obj: {obj.Path}");
                obj.Bus = Bus;
                obj.InitializeSubscribers();
                obj.OnCreate();
                return obj;
            }
        }

        #endregion
    }
}