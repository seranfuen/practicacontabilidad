using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PracticaContabilidad.Helpers
{
    public class ShortToLongAccountCode
    {
        private const int CodeLength = 9;
        private readonly string _input;

        public ShortToLongAccountCode(string input)
        {
            _input = input;
            IsCorrectCode = ValidateInput();
            if (IsCorrectCode)
                LongCode = CovertToLong();
        }

        public string LongCode { get; }
        public bool IsCorrectCode { get; }

        private string CovertToLong()
        {
            var leftSide = GetLeftSide();
            var rightSide = GetRightSide();

            var sb = new StringBuilder(leftSide.PadRight(CodeLength - rightSide.Length, '0'));
            sb.Append(rightSide);
            return sb.ToString();
        }

        private string GetLeftSide()
        {
            return _input.IndexOf(".", StringComparison.Ordinal) >= 0
                ? _input.Substring(0, _input.IndexOf(".", StringComparison.Ordinal))
                : _input;
        }

        private string GetRightSide()
        {
            return _input.IndexOf(".", StringComparison.Ordinal) >= 0
                ? _input.Substring(_input.IndexOf(".", StringComparison.Ordinal) + 1)
                : "";
        }

        private bool ValidateInput()
        {
            return !string.IsNullOrWhiteSpace(_input) &&
                   _input.Length <= CodeLength &&
                   _input.Count(x => x == '.') <= 1 &&
                   _input[0] != '.' &&
                   new Regex("^[0-9.]+$").IsMatch(_input);
        }
    }
}