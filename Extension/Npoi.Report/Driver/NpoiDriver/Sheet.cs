using NPOI.Extend;
using System.Collections;
using System.Collections.Generic;
using NpoiRow = NPOI.SS.UserModel.IRow;
using NpoiSheet = NPOI.SS.UserModel.ISheet;

namespace Npoi.Report.Driver.NpoiDriver
{
    public class Sheet : ISheet
    {
        public NpoiSheet NpoiSheet { get; private set; }

        public Sheet(NpoiSheet npoiSheet)
        {
            NpoiSheet = npoiSheet;
        }

        public IRow this[int rowIndex] => NpoiSheet.GetRow(rowIndex).GetAdapter();

        public string SheetName => NpoiSheet.SheetName;

        public int CopyRows(int start, int end)
        {
            int i = 0;

            i = NpoiSheet.CopyRows(start, end);

            return i;
        }

        public NpoiRow[] InsertRows(int rowIndex, int rowsCount)
        {
            NpoiRow[] rows = null;

            rows = NpoiSheet.InsertRows(rowIndex, rowsCount);

            return rows;
        }

        public int RemoveRows(int start, int end)
        {
            int i = 0;

            i = NpoiSheet.RemoveRows(start, end);

            return i;
        }

        public IEnumerator<IRow> GetEnumerator()
        {
            foreach (NpoiRow npoiRow in NpoiSheet)
            {
                yield return npoiRow.GetAdapter();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object GetOriginal()
        {
            return NpoiSheet;
        }
    }
}