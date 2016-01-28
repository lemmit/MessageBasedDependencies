namespace MessageBasedDependencies.MasterSlaveExample.Messages
{
    public class PrimeOnPositionRequest
    {
        public readonly int Position;
        public PrimeOnPositionRequest(int position)
        {
            Position = position;
        }
    }
}
