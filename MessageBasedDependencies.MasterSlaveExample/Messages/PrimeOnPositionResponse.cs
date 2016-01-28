namespace MessageBasedDependencies.MasterSlaveExample.Messages
{
    public class PrimeOnPositionResponse
    {
        public readonly int Position;
        public readonly int Value;

        public PrimeOnPositionResponse(int position, int value)
        {
            Position = position;
            Value = value;
        }
    }
}
