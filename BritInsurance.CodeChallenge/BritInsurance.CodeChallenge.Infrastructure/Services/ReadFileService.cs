using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BritInsurance.CodeChallenge.Infrastructure.Services
{
    public class ReadFileService : IReadFileService
    {
        public async Task<IEnumerable<string>> GetInstructions(string filePath)
        {
            var result = new List<string>();
            string line;
            using (var stream = new StreamReader(File.OpenRead(filePath)))
            {              
                while (!string.IsNullOrEmpty(line = await stream.ReadLineAsync()))
                {
                     result.Add(line);
                }
            }

            return result;
        }
    }
}
