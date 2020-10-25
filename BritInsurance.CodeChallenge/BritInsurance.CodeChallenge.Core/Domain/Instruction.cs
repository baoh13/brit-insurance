namespace BritInsurance.CodeChallenge.Core.Domain
{
    public class Instruction
    {
        public Instruction(string op, decimal number)
        {
            Operator = op;
            Number = number;
        }

        public string Operator { get; }
        public decimal Number { get; }

        public bool IsApplyNumber
        {
            get
            {
                return Operator.ToLower() == OperatorTypes.Apply;
            }
        }
    }
}
