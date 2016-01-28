using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBasedDependencies.Base;
using MessageBasedDependencies.Base.Buses;
using MessageBasedDependencies.Base.Messages;
using MessageBasedDependencies.Base.Objects;
using MessageBasedDependencies.MasterSlaveExample.Messages;
using MessageBasedDependencies.MasterSlaveExample.Objects;

namespace MessageBasedDependencies.MasterSlaveExample
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any()) { ShowHelp(); }
            var settings = GetMasterWorkerSlaveSettings(args);
            Debug.WriteLine(
                        $"Starting computation [{settings.ComputationLowerLimit}, {settings.ComputationUpperLimit}]" +
                        $" with  + {(settings.AsyncBus ? "async" : "sync")} bus" +
                        $" and {settings.SlaveObjectsCount} slave worker(s)"
                );
            //Create system
            var os = new ObjectSystem("/testSystem", new InMemoryBus(async: settings.AsyncBus));
            //Create master
            os.Create(() => new MasterWorkerObject(settings.SlaveObjectsCount));

            //Create subscriber to catch results sent from master
            var sub = os.Create(() => new ActionSubscriberObject<LogMessage>(msg =>
                   Console.WriteLine(
                       $"{settings.ComputationLowerLimit},"+
                       $"{settings.ComputationUpperLimit}," +
                       $"{settings.AsyncBus}," +
                       $"{settings.SlaveObjectsCount}," +
                       $"{msg.Message},"
                       )
               ));

            //Create generic obj and send some work parameters to the master
            var genericPub = os.Create(() => new GenericPublisherObject());
            var work = Enumerable.Range((int)settings.ComputationLowerLimit, (int)settings.ComputationUpperLimit).ToArray();
            genericPub.Publish(new PrimeOnPositionsToSumRequest(work));
            
            Console.ReadLine();
        }

        private static void ShowHelp()
        {
            Console.WriteLine(
                "You can run this program with other parameters: \n"+
                "sample.exe <lower> <upper> <nrOfSlaves> <shouldUseAsyncBus> \n" +
                "Example: sample.exe 1 100 8 true"
                );
        }

        private static MasterSlaveSettings GetMasterWorkerSlaveSettings(string[] args)
        {
            uint masterWorkerSlaveObjectsCount = 8;
            var busShouldWorkAsync = true;
            uint lowerLimit = 1;
            uint upperLimit = 500;
            
            var argc = args.Count();
            if (argc > 0) uint.TryParse(args[0], out lowerLimit);
            if (argc > 1) uint.TryParse(args[1], out upperLimit);
            if (argc > 2) uint.TryParse(args[2], out masterWorkerSlaveObjectsCount);
            if (argc > 3) bool.TryParse(args[3], out busShouldWorkAsync);

            return new MasterSlaveSettings()
            {
                SlaveObjectsCount = masterWorkerSlaveObjectsCount,
                AsyncBus = busShouldWorkAsync,
                ComputationLowerLimit = lowerLimit,
                ComputationUpperLimit = upperLimit
            };
        }
        private class MasterSlaveSettings
        {
            public uint SlaveObjectsCount { get; set; }
            public bool AsyncBus { get; set; }
            public uint ComputationLowerLimit { get; set; }
            public uint ComputationUpperLimit { get; set; }
        }
    }
}
