using Microsoft.Extensions.DependencyInjection;
using System;
using BritInsurance.CodeChallenge.Infrastructure;
using System.Threading.Tasks;

namespace BritInsurance.CodeChallenge.ConsoleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider();

            await serviceProvider.GetService<Startup>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddInfrastructure();

            services.AddTransient<Startup>();

            return services;
        }
    }
}
