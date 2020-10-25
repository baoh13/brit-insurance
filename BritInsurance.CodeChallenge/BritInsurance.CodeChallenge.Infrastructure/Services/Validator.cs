using BritInsurance.CodeChallenge.Core.Domain;
using BritInsurance.CodeChallenge.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BritInsurance.CodeChallenge.Infrastructure.Services
{
    public class Validator : IValidator
    {
        readonly List<string> INSTRUCTION_TYPES = new List<string>() 
        { 
            OperatorTypes.Add, 
            OperatorTypes.Subtract, 
            OperatorTypes.Multiply, 
            OperatorTypes.Divide, 
            OperatorTypes.Apply 
        };

        public void ValidateLastLine(string line)
        {
            if (!line.ToLower().Contains(OperatorTypes.Apply))
            {
                throw new InputFileInvalidFormatException($"Invalid last instruction '{line}'. It should be Apply and a number.");
            }

            ValidateLine(line);
        }

        public void Validate(IEnumerable<Instruction> instructions)
        {
            if (instructions == null)
                throw new ArgumentNullException(nameof(instructions));

            if (instructions.Where(i => i.Operator.ToLower() == OperatorTypes.Apply).Count() > 1)
                throw new InputFileInvalidFormatException("Too many Apply instructions");
        }

        public void Validate(string line)
        {
            ValidateLine(line);
        }

        private void ValidateLine(string line)
        {
            var segments = line.Trim().Split(' ');

            if ((segments.Length == 2 && string.IsNullOrEmpty(segments[1]))
                 || segments.Length != 2
                 || !decimal.TryParse(segments[1], out decimal number))
            {
                throw new InputFileInvalidFormatException($"Invalid instruction '{line}'");
            }

            else if (!INSTRUCTION_TYPES.Contains(segments[0].ToLower()))
            {
                throw new InputFileInvalidFormatException($"Unsupported instruction type '{segments[0]}'");
            }

            else if (segments[0].ToLower() == OperatorTypes.Divide && decimal.Parse(segments[1]) == 0)
            {
                throw new InputFileInvalidFormatException($"Dividing by 0 instruction '{line}'");
            }
        }
    }
}
