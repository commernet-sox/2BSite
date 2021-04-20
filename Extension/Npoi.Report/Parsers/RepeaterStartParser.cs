using System.Text.RegularExpressions;

namespace Npoi.Report.Parsers
{
    public sealed class RepeaterStartParser : RegexParser
    {
        private static readonly Regex regex = new Regex(@"(?<=\<\[)([\w]*)(?=\])");

        public override Regex Regex => regex;
    }
}