using BritInsurance.CodeChallenge.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BritInsurance.CodeChallenge.Infrastructure.Services
{
    public interface IInstructionSetService
    {
        Task<InstructionSet> GetInstructionSet(string filePath);
    }
}
