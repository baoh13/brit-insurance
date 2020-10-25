using BritInsurance.CodeChallenge.Core.Domain;
using BritInsurance.CodeChallenge.Infrastructure.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BritInsurance.CodeChallenge.Tests
{
    public class InstructionSetServiceTests
    {
        private Mock<IValidator> mockValidator = new Mock<IValidator>();

        [Fact]
        public void ItValidatesAllInstructionLines()
        {
            var lines = new List<string>() { "add 3", "multiply 5", "apply 3" };

            var instructionSetService = new InstructionSetService(mockValidator.Object);

            instructionSetService.GetInstructionSet(lines);

            mockValidator.Verify(v => v.Validate(It.IsAny<string>()), Times.Exactly(3));
        }

        [Theory]
        [InlineData("add 3")]
        [InlineData("subtract 3")]
        [InlineData("divide 3")]
        [InlineData("multiply 3")]
        public void ItValidatesInstructionLine(string instruction)
        {
            var lines = new List<string>() { instruction };

            var instructionSetService = new InstructionSetService(mockValidator.Object);

            var validatedInput = "";
            mockValidator.Setup(v => v.Validate(It.IsAny<string>())).Callback<string>(l => validatedInput = l);                  

            instructionSetService.GetInstructionSet(lines);

            Assert.Equal(instruction, validatedInput);
        }

        [Theory]
        [InlineData(" add 3 ", "add 3")]
        [InlineData(" subtract 3 ", "subtract 3")]
        [InlineData("  divide 3 ", "divide 3")]
        [InlineData("  multiply 3  ", "multiply 3")]
        public void ItValidatesTrimmedInstructionLine(string instruction, string validatedInstruction)
        {
            var lines = new List<string>() { instruction };

            var instructionSetService = new InstructionSetService(mockValidator.Object);

            var validatedInput = "";
            mockValidator.Setup(v => v.Validate(It.IsAny<string>())).Callback<string>(l => validatedInput = l);

            instructionSetService.GetInstructionSet(lines);

            Assert.Equal(validatedInstruction, validatedInput);
        }

        [Fact]
        public void ItValidatesLastInstructinoLineOnce()
        {
            var lines = new List<string>() { "add 3", "multiply 5", "apply 3" };

            var instructionSetService = new InstructionSetService(mockValidator.Object);

            instructionSetService.GetInstructionSet(lines);

            mockValidator.Verify(v => v.ValidateLastLine(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData("apply 3")]
        [InlineData("apply 1")]
        [InlineData("apply 2")]
        public void ItValidatesLastInstructionLine(string applyLine)
        {
            var lines = new List<string>() { "add 3", "multiply 5", applyLine };

            var instructionSetService = new InstructionSetService(mockValidator.Object);

            var expectedLastline = "";
            mockValidator.Setup(v => v.ValidateLastLine(It.IsAny<string>()))
                         .Callback<string>(l => expectedLastline = l);

            instructionSetService.GetInstructionSet(lines);

            mockValidator.Verify(v => v.ValidateLastLine(It.IsAny<string>()), Times.Once);
            Assert.Equal(applyLine, expectedLastline);
        }

        [Fact]
        public void ItValidatesInstructions()
        {
            var lines = new List<string>() { "add 3", "multiply 5", "apply 3" };

            var instructionSetService = new InstructionSetService(mockValidator.Object);

            instructionSetService.GetInstructionSet(lines);

            mockValidator.Verify(v => v.Validate(It.IsAny<List<Instruction>>()), Times.Once);
        }

        [Fact]
        public void ItReturnsExpectedSetOfInstructions()
        {
            var lines = new List<string>() { "add 3", "multiply 5", "divide 1", "apply 3" };

            var instructionSetService = new InstructionSetService(mockValidator.Object);

            var set = instructionSetService.GetInstructionSet(lines);
            var addInstruction = set.Instructions.ElementAt(0);
            var multiplyInstruction = set.Instructions.ElementAt(1);
            var divideInstruction = set.Instructions.ElementAt(2);

            Assert.Equal(3, set.ApplyNumber);
            Assert.Equal(3, set.Instructions.Count());

            Assert.NotNull(addInstruction);
            Assert.Equal(3, addInstruction.Number);
            Assert.False(addInstruction.IsApplyNumber);

            Assert.NotNull(multiplyInstruction);
            Assert.Equal(5, multiplyInstruction.Number);
            Assert.False(multiplyInstruction.IsApplyNumber);

            Assert.NotNull(divideInstruction);
            Assert.Equal(1, divideInstruction.Number);
            Assert.False(divideInstruction.IsApplyNumber);
        }

        [Fact]
        public void ItTrimsLinesInTheInputDataFile()
        {
            var lines = new List<string>() { "add 3 \r \n", "subtract 5 ", "apply 3 \r " };

            var instructionSetService = new InstructionSetService(mockValidator.Object);

            var set = instructionSetService.GetInstructionSet(lines);
            var addInstruction = set.Instructions.SingleOrDefault(i => i.Operator == "add");
            var subtractInstruction = set.Instructions.SingleOrDefault(i => i.Operator == "subtract");

            Assert.Equal(3, set.ApplyNumber);
            Assert.Equal(2, set.Instructions.Count());

            Assert.NotNull(addInstruction);
            Assert.Equal(3, addInstruction.Number);
            Assert.False(addInstruction.IsApplyNumber);

            Assert.NotNull(subtractInstruction);
            Assert.Equal(5, subtractInstruction.Number);
            Assert.False(subtractInstruction.IsApplyNumber);
        }
    }
}
