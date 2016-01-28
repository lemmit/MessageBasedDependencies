namespace MessageBasedDependencies.MasterSlaveExample.Messages
{
    public class PrimeOnPositionsToSumRequest
    {
        public readonly int[] Positions;
        public PrimeOnPositionsToSumRequest(int[] positions)
        {
            Positions = positions;
        }
    }
}
