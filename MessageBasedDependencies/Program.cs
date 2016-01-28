using System.Linq;
using System.Threading.Tasks;
using MessageBasedDependencies.Base;
using MessageBasedDependencies.Base.Buses;
using MessageBasedDependencies.Base.Objects;
using MessageBasedDependencies.SampleObjects;

namespace MessageBasedDependencies
{
    class Program
    {
        static void Main()
        {
            //Create system
            var os = new ObjectSystem("/testSystem", new InMemoryBus(async: true));
            var log = os.Create(() => new LoggerObject(), "logger/1");
            var echo = os.Create(() => new EchoObject(), "echo");
            //send ping to echo actor from 50 different objects
            var taskArray = Enumerable
                .Range(0, 50)
                .Select(i => os.Create(() => new AskAnswerPingObject()).Ping("/testSystem/echo"))
                .ToArray();
            Task.WaitAll(taskArray);

            var log2 = os.Create(() => new LoggerObject(), "logger/2");
            var log3 = os.Create(() => new LoggerObject(), "logger/3");
            var slbp = os.Create(() => new SendingLogByPathObject());
            //send log messages to loggers, specify them with path
            slbp.SendLogByPath("/testSystem/logger/2");
            slbp.SendLogByPath("/testSystem/logger/2");
            slbp.SendLogByPath("/testSystem/logger/2");
            slbp.SendLogByPath("/testSystem/logger/3");
            slbp.SendLogByPath("/testSystem/logger/3");
            slbp.SendLogByPath("/testSystem/logger/3");
        }
    }
}
