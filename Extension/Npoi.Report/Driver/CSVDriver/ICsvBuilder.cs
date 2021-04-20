using System.Text;

namespace Npoi.Report.Driver.CSVDriver
{
    internal interface ICsvBuilder
    {
        void AppendTo(StringBuilder builder);
    }
}