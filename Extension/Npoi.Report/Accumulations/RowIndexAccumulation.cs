﻿namespace Npoi.Report.Accumulations
{
    public class RowIndexAccumulation : Accumulation
    {
        public int GetCurrentRowIndex(int sourceRowIndex)
        {
            return Value + sourceRowIndex;
        }
    }
}