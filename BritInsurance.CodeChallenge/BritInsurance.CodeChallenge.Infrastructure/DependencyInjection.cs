using BritInsurance.CodeChallenge.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BritInsurance.CodeChallenge.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ICalculationService, CalculationService>();
            services.AddTransient<IInstructionSetService, InstructionSetService>();
            services.AddTransient<IReadFileService, ReadFileService>();
            services.AddTransient<IValidator, Validator>();
            return services;
        }
    }
}
