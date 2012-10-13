using System.Threading;
using MemBus.Configurators;
using MemBus.Publishing;
using MemBus.Support;
using MemBus.Tests.Help;
using System.Linq;
using MemBus.Tests.Frame;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;

namespace MemBus.Tests.Publishing
{
    [TestClass]
    public class Awaitable_publish
    {
        [TestMethod]
        public async Task using_the_awaitable_publish()
        {
            var b = BusSetup.StartWith<Conservative>().Construct();
            var messageReceived = false;
            b.Subscribe((string h) => messageReceived = true);
            await b.PublishAsync("Hello");
            Assert.IsTrue(messageReceived);
        }


    }
}