using System.Threading.Tasks;

namespace BritInsurance.CodeChallenge.Infrastructure.Services
{
    public interface ICalculationService
    {
        Task<decimal> Run(string filePath);
    }
}
