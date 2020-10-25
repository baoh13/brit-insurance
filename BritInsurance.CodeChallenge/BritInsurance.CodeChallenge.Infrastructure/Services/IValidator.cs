using BritInsurance.CodeChallenge.Core.Domain;
using System.Collections.Generic;

namespace BritInsurance.CodeChallenge.Infrastructure.Services
{
    public interface IValidator
    {
        void ValidateLastLine(string line);
        void Validate(string line);
        void Validate(IEnumerable<Instruction> instructions);
    }
}
