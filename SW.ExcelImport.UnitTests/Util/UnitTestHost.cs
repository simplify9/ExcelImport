using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.Pmm.UnitTests
{
    public class UnitTestHost
    {
        readonly IServiceProvider _serviceProvider;

        public UnitTestHost(Action<IServiceCollection> diBindings)
        {
            var serviceCollection = new ServiceCollection();
            diBindings(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public void Run(Action<IServiceProvider> scopeLogic)
        {
            var scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var scopeServiceProvider = scope.ServiceProvider;
                scopeLogic(scopeServiceProvider);
            }
        }

        public async Task RunAsync(Func<IServiceProvider,Task> scopeLogic)
        {
            var scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var scopeServiceProvider = scope.ServiceProvider;
                await scopeLogic(scopeServiceProvider);
            }
        }

        public void Run<TTestSubject>(Action<TTestSubject> subjectTestLogic)
        {
            Run((serviceProvider) =>
            {
                var testSubject = serviceProvider.GetService<TTestSubject>();
                subjectTestLogic(testSubject);
            });
        }

        public Task RunAsync<TTestSubject>(Func<TTestSubject,Task> subjectTestLogic)
        {
            return RunAsync(async (serviceProvider) =>
            {
                var testSubject = serviceProvider.GetService<TTestSubject>();
                await subjectTestLogic(testSubject);
            });
        }
    }
}
