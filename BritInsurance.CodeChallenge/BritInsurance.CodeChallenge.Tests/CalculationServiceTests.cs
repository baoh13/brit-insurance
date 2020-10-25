using BritInsurance.CodeChallenge.Core.Domain;
using BritInsurance.CodeChallenge.Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace BritInsurance.CodeChallenge.Tests
{
    public class CalculationServiceTests
    {
        private Mock<IReadFileService> _mockReadFileService = new Mock<IReadFileService>();
        private Mock<IInstructionSetService> _mockInstructionSetService = new Mock<IInstructionSetService>();

        [Fact]
        public async Task ItCallsReadFileServiceOnceToReadInputFile()
        {
            var instructionSet = new InstructionSet()
            {
                ApplyNumber = 3,
                Instructions = new List<Instruction>() { new Instruction("add", 3) }
            };

            _mockInstructionSetService.Setup<InstructionSet>(v => v.GetInstructionSet(It.IsAny<IEnumerable<string>>()))
                                      .Returns(instructionSet);
            var calculationService = new CalculationService(_mockReadFileService.Object, _mockInstructionSetService.Object);

            await calculationService.Run("filePath");

            _mockReadFileService.Verify(s => s.GetInstructions(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData("File1Path")]
        [InlineData("File2Path")]
        [InlineData("File3Path")]
        public async Task ItCallsReadFileServiceOnceToReadInputFileWithExpectedFilePath(string inputPath)
        {
            var instructionSet = new InstructionSet()
            {
                ApplyNumber = 3,
                Instructions = new List<Instruction>() { new Instruction("add", 3) }
            };

            _mockInstructionSetService.Setup<InstructionSet>(v => v.GetInstructionSet(It.IsAny<IEnumerable<string>>()))
                                      .Returns(instructionSet);


            var expectedPath = "";
            _mockReadFileService.Setup(v => v.GetInstructions(It.IsAny<string>()))
                                      .Callback<string>(p => expectedPath = p);

            var calculationService = new CalculationService(_mockReadFileService.Object, _mockInstructionSetService.Object);


            await calculationService.Run(inputPath);


            _mockReadFileService.Verify(s => s.GetInstructions(It.IsAny<string>()), Times.Once);

            Assert.Equal(inputPath, expectedPath);
        }

        [Fact]
        public async Task ItCallsInstructionSetServiceOnceToGetInstructionSet()
        {
            var instructionSet = new InstructionSet()
            {
                ApplyNumber = 3,
                Instructions = new List<Instruction>() { new Instruction("add", 3) }
            };

            var lines = new List<string>() { "add 3", "apply 3" };

            _mockInstructionSetService.Setup<InstructionSet>(v => v.GetInstructionSet(It.IsAny<IEnumerable<string>>()))
                                      .Returns(instructionSet);

            _mockReadFileService.Setup(v => v.GetInstructions(It.IsAny<string>()))
                                .ReturnsAsync(lines);

            var calculationService = new CalculationService(_mockReadFileService.Object, _mockInstructionSetService.Object);

            await calculationService.Run("filePath");

            _mockInstructionSetService.Verify(s => s.GetInstructionSet(It.IsAny<List<string>>()), Times.Once);
        }

        [Fact]
        public async Task ItThrowsFileIONotFoundException()
        {
            _mockReadFileService.Setup(s => s.GetInstructions(It.IsAny<string>()))
                                .ThrowsAsync(new FileNotFoundException());

            var calculationService = new CalculationService(_mockReadFileService.Object, _mockInstructionSetService.Object);

            Func<Task> act = () => calculationService.Run("filePath");

            var ex = await Assert.ThrowsAsync<FileNotFoundException>(act);
            Assert.NotNull(ex);
            Assert.Contains("Unable to find the specified file", ex.Message);
        }

        [Fact]
        public async Task ItReturnsExpectedResult()
        {
            var instructionSet = new InstructionSet() { 
                ApplyNumber = 3,
                Instructions = new List<Instruction>()
                {
                    new Instruction("add", 3),
                    new Instruction("add", 3)
                }
            };

            _mockInstructionSetService.Setup<InstructionSet>(v => v.GetInstructionSet(It.IsAny<IEnumerable<string>>()))
                                      .Returns(instructionSet);

            var calculationService = new CalculationService(
                _mockReadFileService.Object,
                _mockInstructionSetService.Object);

            var result = await calculationService.Run("filePath");

            Assert.Equal(9, result);
        }

        [Fact]
        public async Task ItReturnsExpectedResult_2()
        {
            var instructionSet = new InstructionSet()
            {
                ApplyNumber = 3,
                Instructions = new List<Instruction>()
                {
                    new Instruction("add", 3),
                    new Instruction("Add", 3),
                    new Instruction("multiply", 5),
                    new Instruction("multiply", 2),
                    new Instruction("divide", 4),
                    new Instruction("divide", 2),
                }
            };

            _mockInstructionSetService.Setup<InstructionSet>(v => v.GetInstructionSet(It.IsAny<IEnumerable<string>>()))
                                      .Returns(instructionSet);

            var calculationService = new CalculationService(
                _mockReadFileService.Object,
                _mockInstructionSetService.Object);

            var result = await calculationService.Run("filePath");

            Assert.Equal(11.25m, result);
        }
    }
}
