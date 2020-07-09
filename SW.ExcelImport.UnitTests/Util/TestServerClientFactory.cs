using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;


namespace SW.ExcelImport.UnitTests
{
    public static class TestServerClientFactory
    {
        readonly static TestServer server = new TestServer(new WebHostBuilder()
            .UseEnvironment("UnitTesting")
            .UseStartup<TestStartup>());

        public static HttpClient GetClient()
        {

            return server.CreateClient();
        }

        public static TestServer GetServer()
        {
            return server;
        }

        public static IServiceProvider GetServiceProvider()
        {
            return server.Host.Services ;
        }


    }
}
