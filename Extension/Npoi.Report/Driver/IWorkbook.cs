using System.Collections.Generic;

namespace Npoi.Report.Driver
{
    public interface IWorkbook : IEnumerable<ISheet>
    {
        ISheet this[string sheetName]
        {
            get;
        }

        byte[] SaveToBuffer();
    }
}