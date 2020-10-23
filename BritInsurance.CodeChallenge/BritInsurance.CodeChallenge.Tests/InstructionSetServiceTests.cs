using BritInsurance.CodeChallenge.Infrastructure.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BritInsurance.CodeChallenge.Tests
{
    public class InstructionSetServiceTests
    {
        private Mock<IReadFileService> _mockReadFileService = new Mock<IReadFileService>();

        [Fact]
        public async Task ItReturnExpectedSetOfInstructions()
        {
            var lines = new List<string>() { "add 3", "multiply 5", "apply 3" };

            _mockReadFileService.Setup( s => s.GetInstructions(It.IsAny<string>()))
                                .ReturnsAsync(lines);

            var instructionSetService = new InstructionSetService(_mockReadFileService.Object);

            var set = await instructionSetService.GetInstructionSet("filePath");

            Assert.Equal(3, set.ApplyNumber);
            Assert.Equal(2, set.Instructions.Count());
            Assert.Equal("add", set.Instructions.ElementAt(0).Operator);
            Assert.Equal(3, set.Instructions.ElementAt(0).Number);
            Assert.Equal("multiply", set.Instructions.ElementAt(1).Operator);
            Assert.Equal(5, set.Instructions.ElementAt(1).Number);
        }
    }
}
