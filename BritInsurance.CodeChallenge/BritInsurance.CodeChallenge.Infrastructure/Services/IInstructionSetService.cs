using BritInsurance.CodeChallenge.Core.Domain;
using System.Collections.Generic;

namespace BritInsurance.CodeChallenge.Infrastructure.Services
{
    public interface IInstructionSetService
    {
        InstructionSet GetInstructionSet(IEnumerable<string> lines);
    }
}
