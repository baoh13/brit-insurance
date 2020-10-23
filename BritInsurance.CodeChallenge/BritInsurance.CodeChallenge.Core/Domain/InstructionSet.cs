using System.Collections.Generic;

namespace BritInsurance.CodeChallenge.Core.Domain
{
    public class InstructionSet
    {
        public IEnumerable<Instruction> Instructions { get; set; }
        public decimal ApplyNumber { get; set; }
    }
}
