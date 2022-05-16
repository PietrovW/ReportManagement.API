using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using TechTalk.SpecFlow;

namespace ReportManagement.Api.Test.Acceptance.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private static IWebHost _host;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Program>();
            TestServer testServer = new TestServer(builder);
            _host= testServer.Host;
            _host.Start();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _host.StopAsync().Wait();
        }
    }
}
