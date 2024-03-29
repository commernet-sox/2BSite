﻿using System.Collections.Generic;

namespace Npoi.Report.Driver
{
    public interface ISheet : IEnumerable<IRow>
    {
        string SheetName { get; }

        IRow this[int rowIndex]
        {
            get;
        }

        int CopyRows(int start, int end);

        int RemoveRows(int start, int end);
    }
}