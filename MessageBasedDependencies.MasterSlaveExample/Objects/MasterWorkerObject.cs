using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MessageBasedDependencies.Base;
using MessageBasedDependencies.Base.DeliveryStrategies;
using MessageBasedDependencies.Base.Messages;
using MessageBasedDependencies.MasterSlaveExample.Messages;

namespace MessageBasedDependencies.MasterSlaveExample.Objects
{
    public class MasterWorkerObject : 
        BaseObject,
        IPublisher<PrimeOnPositionRequest>,
        IPublisher<LogMessage>,
        ISubscriber<PrimeOnPositionResponse>,
        ISubscriber<PrimeOnPositionsToSumRequest>
    {
        private readonly uint _maxNumberOfWorkers;
        public MasterWorkerObject(uint maxNumberOfWorkers = 20)
        {
            _maxNumberOfWorkers = maxNumberOfWorkers;
        }
        public void Receive(PrimeOnPositionsToSumRequest message)
        {
            var sw = new Stopwatch();
            sw.Start();
            var calc = RunCalculation(message.Positions);
            sw.Stop();
            this.Publish(new LogMessage($"{calc},{sw.Elapsed}"));
        }

        public void Receive(PrimeOnPositionResponse message)
        {
            //
        }

        private int RunCalculation(int[] positions)
        {
            var loadBalanceDeliveryStrategy = new RandomLoadBalanceDeliveryStrategy();
            var nrOfWorkers = (int)Math.Min(positions.Length, _maxNumberOfWorkers);
            Debug.WriteLine($"Creating {nrOfWorkers} workers in Master.");
            var workers = Enumerable
                .Range(0, nrOfWorkers)
                .Select(i => Create(() => new WorkerObject()))
                .ToList();
            var workerTasks = Enumerable
                .Range(0, positions.Length)
                .Select(i =>  this.Ask<PrimeOnPositionRequest, PrimeOnPositionResponse, MasterWorkerObject>(
                                    new PrimeOnPositionRequest(positions[i]),
                                    loadBalanceDeliveryStrategy
                ))
                .ToArray();
            Task.WaitAll(workerTasks);
            return workerTasks.Sum(task => task.Result.Value);
        }
    }
}
