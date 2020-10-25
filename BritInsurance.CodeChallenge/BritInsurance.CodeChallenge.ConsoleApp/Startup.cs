using BritInsurance.CodeChallenge.Infrastructure.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BritInsurance.CodeChallenge.ConsoleApp
{
    public class Startup
    {
        private readonly ICalculationService _calculationService;

        public Startup(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        public async Task Run()
        {
            try
            {
                Console.WriteLine("Hello World!!");
                var result = await _calculationService.Run(@"data.csv");

                Console.WriteLine(result);
            }
            catch (FileNotFoundException ex)
            {

            }
            catch (AggregateException ex)
            {
                var error = "";

                ex.InnerExceptions.AsParallel().ForAll(innerEx =>
                {
                    error += $"Error: {innerEx.Message} \n";
                });

                Console.WriteLine(error);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
