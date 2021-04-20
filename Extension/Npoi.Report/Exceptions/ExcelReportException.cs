using System;

namespace Npoi.Report.Exceptions
{
    public class ExcelReportException : ApplicationException
    {
        public ExcelReportException(string message) : base(message)
        {
        }
    }
}