using BritInsurance.CodeChallenge.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace BritInsurance.CodeChallenge.Infrastructure.Services
{
    public class InstructionSetService : IInstructionSetService
    {
        private readonly IReadFileService _readFileService;

        public InstructionSetService(IReadFileService readFileService)
        {
            _readFileService = readFileService;
        }

        public async Task<InstructionSet> GetInstructionSet(string filePath)
        {
            var lines = (await _readFileService.GetInstructions(filePath)).ToList();

            var intructionSet = new InstructionSet();
            var instructions = new List<Instruction>();

            for (int i = 0; i < lines.Count; i++){            

                var line = lines[i].Trim();
                
                var segments = line.Split(' ');

                if (segments[0].Equals("apply"))
                {
                    intructionSet.ApplyNumber = decimal.Parse(segments[1]);
                }
                else
                {
                    instructions.Add(new Instruction()
                    {
                        Number = decimal.Parse(segments[1]),
                        Operator = segments[0]
                    });
                }

            }
            intructionSet.Instructions = instructions;

            return intructionSet;
        }
    }
}
