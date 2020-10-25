using BritInsurance.CodeChallenge.Core.Domain;
using BritInsurance.CodeChallenge.Infrastructure.Exceptions;
using BritInsurance.CodeChallenge.Infrastructure.Services;
using System.Collections.Generic;
using Xunit;

namespace BritInsurance.CodeChallenge.Tests
{
    public class ValidatorTests
    {
        [Theory]
        [InlineData("add ", "Invalid instruction 'add '")]
        [InlineData("add 3 asd", "Invalid instruction 'add 3 asd'")]
        [InlineData("add asd", "Invalid instruction 'add asd'")]
        [InlineData("ad 3", "Unsupported instruction type 'ad'")]
        [InlineData("apply 3 asd", "Invalid instruction 'apply 3 asd'")]
        public void ItThrowsCustomExceptionWhenMissingTheOperatorNumber(string inputLine, string errorMsg)
        {
            var line = inputLine;

            var validator = new Validator();

            var ex = Assert.Throws<InputFileInvalidFormatException>(() => validator.Validate(line));

            Assert.Contains(errorMsg, ex.Message);
        }

        [Fact]
        public void ItThrowsCustomExceptionWithTwoManyApplyInstructions()
        {
            var instructions = new List<Instruction>()
            {
                new Instruction("apply", 3),
                new Instruction("apply", 2)
            };

            var validator = new Validator();

            var ex = Assert.Throws<InputFileInvalidFormatException>(() => validator.Validate(instructions));
            Assert.Contains("Too many Apply instructions", ex.Message);
        }

        [Fact]
        public void ItThrowsCustomExceptionWithInvalidLastLine()
        {
            var lines = "add 3";

            var validator = new Validator();

            var ex = Assert.Throws<InputFileInvalidFormatException>(() => validator.ValidateLastLine(lines));
            Assert.Contains("Invalid last instruction 'add 3'. It should be Apply and a number.", ex.Message);
        }

        [Fact]
        public void ItThrowsCustomExceptionDividingBy0()
        {
            var lines = "divide 0";

            var validator = new Validator();

            var ex = Assert.Throws<InputFileInvalidFormatException>(() => validator.Validate(lines));
            Assert.Contains("Dividing by 0 instruction 'divide 0'", ex.Message);
        }

        [Theory]
        [InlineData("add")]
        [InlineData("subtract")]
        [InlineData("divide")]
        [InlineData("multiply")]
        public void NoExceptionIfLineHasSupportedOperatorAndANumber(string op)
        {
            var lines = $"{op} 3";

            var validator = new Validator();

            validator.Validate(lines);
        }

        [Theory]
        [InlineData("Add")]
        [InlineData("ADD")]
        [InlineData("Subtract")]
        [InlineData("SUBTRACT")]
        [InlineData("Divide")]
        [InlineData("Multiply")]
        public void NoExceptionIfLineHasSupportedCaseSensitiveOperatorAndANumber(string op)
        {
            var lines = $"{op} 3";

            var validator = new Validator();

            validator.Validate(lines);
        }

        [Theory]
        [InlineData("apply 3")]
        [InlineData("apply 3 ")]
        public void NoExceptionIfLastLineHasApplyAndANumber(string applyLine)
        {
            var lines = applyLine;

            var validator = new Validator();

            validator.ValidateLastLine(lines);
        }
    }
}
