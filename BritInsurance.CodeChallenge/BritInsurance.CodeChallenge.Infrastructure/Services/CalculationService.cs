using BritInsurance.CodeChallenge.Core.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace BritInsurance.CodeChallenge.Infrastructure.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IReadFileService _readFileService;
        private readonly IInstructionSetService _instructionSetService;

        public CalculationService(
            IReadFileService readFileService,
            IInstructionSetService instructionSetService)
        {
            _readFileService = readFileService;
            _instructionSetService = instructionSetService;
        }
        public async Task<decimal> Run(string filePath)
        {
            var lines = await _readFileService.GetInstructions(filePath);

            var instructionSet = _instructionSetService.GetInstructionSet(lines);

            var result = Calculate(instructionSet);

            return result;
        }

        private decimal Calculate(InstructionSet instructionSet)
        {
            var result = instructionSet.ApplyNumber;
            var instructions = instructionSet.Instructions;
            Instruction instruction;

            for (int i = 0; i < instructions.Count(); i++)
            {
                instruction = instructions.ElementAt(i);
                switch (instruction.Operator.ToLower())
                {
                    case OperatorTypes.Add:
                        result += instruction.Number;
                        break;
                    case OperatorTypes.Subtract:
                        result -= instruction.Number;
                        break;
                    case OperatorTypes.Multiply:
                        result *= instruction.Number;
                        break;
                    case OperatorTypes.Divide:
                        result /= instruction.Number;
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
    }
}
