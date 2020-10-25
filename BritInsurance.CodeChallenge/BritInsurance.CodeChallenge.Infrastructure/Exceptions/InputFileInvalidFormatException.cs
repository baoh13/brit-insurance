using System;

namespace BritInsurance.CodeChallenge.Infrastructure.Exceptions
{
    public class InputFileInvalidFormatException: Exception
    {
        public InputFileInvalidFormatException(string error): base(error)
        {}
    }
}
