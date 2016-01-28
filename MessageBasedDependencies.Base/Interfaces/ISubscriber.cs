namespace MessageBasedDependencies.Base
{
    public interface ISubscriber
    {
        string Path { get; }
    }

    public interface ISubscriber<T> : ISubscriber
    {
        void Receive(T message);
    }
}