using System.Collections.Generic;
using System.Threading.Tasks;

namespace BritInsurance.CodeChallenge.Infrastructure.Services
{
    public interface IReadFileService
    {
        Task<IEnumerable<string>> GetInstructions(string filePath);
    }
}
