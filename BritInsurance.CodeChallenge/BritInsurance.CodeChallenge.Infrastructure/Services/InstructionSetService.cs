using BritInsurance.CodeChallenge.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace BritInsurance.CodeChallenge.Infrastructure.Services
{
    public class InstructionSetService : IInstructionSetService
    {
        private readonly IValidator _validator;

        public InstructionSetService(IValidator validator)
        {
            _validator = validator;
        }

        public InstructionSet GetInstructionSet(IEnumerable<string> inputLines)
        {
            var instructionSet = new InstructionSet();
            var instructions = new List<Instruction>();

            _validator.ValidateLastLine(inputLines.Last());

            var orderedInstructions = inputLines.AsParallel().AsOrdered().Select(inputLine =>
            {
                var line = inputLine.Trim();
                _validator.Validate(line);

                var segments = line.Split(' ');
                var number = decimal.Parse(segments[1]);

                var instruction = new Instruction(op: segments[0], number);

                if (instruction.IsApplyNumber)
                    instructionSet.ApplyNumber = instruction.Number;

                return instruction;
            }).ToList();

            _validator.Validate(orderedInstructions);

            instructionSet.Instructions = orderedInstructions.Where(i => i.Operator.ToLower() != OperatorTypes.Apply);
            return instructionSet;
        }
    }
}
