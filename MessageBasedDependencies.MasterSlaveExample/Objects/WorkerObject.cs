using MessageBasedDependencies.Base;
using MessageBasedDependencies.MasterSlaveExample.Messages;

namespace MessageBasedDependencies.MasterSlaveExample.Objects
{
    public class WorkerObject
        : BaseObject,
        ISubscriber<PrimeOnPositionRequest>,
        IPublisher<PrimeOnPositionResponse>
    {
        public void Receive(PrimeOnPositionRequest message)
        {
            var found = 0;
            var lastPrime = 0;
            for (var i = 1; found < message.Position; i++)
            {
                if (!IsPrimeNumber(i))
                    continue;                
                found++;
                lastPrime = i;
            }
            this.Answer(message, new PrimeOnPositionResponse(message.Position, lastPrime));
        }

        private bool IsPrimeNumber(int num)
        {
            bool bPrime = true;
            int factor = num / 2;

            int i = 0;

            for (i = 2; i <= factor; i++)
            {
                if ((num % i) == 0)
                    bPrime = false;
            }
            return bPrime;
        }
    }
}
