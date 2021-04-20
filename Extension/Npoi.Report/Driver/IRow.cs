using System.Collections.Generic;

namespace Npoi.Report.Driver
{
    public interface IRow : IEnumerable<ICell>
    {
        ICell this[int columnIndex]
        {
            get;
        }
    }
}