using System.Collections;
using System.Collections.Generic;
using NpoiCell = NPOI.SS.UserModel.ICell;
using NpoiRow = NPOI.SS.UserModel.IRow;

namespace Npoi.Report.Driver.NpoiDriver
{
    public class Row : IRow
    {
        public NpoiRow NpoiRow { get; private set; }

        public Row(NpoiRow npoiRow)
        {
            NpoiRow = npoiRow;
        }

        public ICell this[int columnIndex]
        {
            get
            {
                var cell = NpoiRow.GetCell(columnIndex);
                if (cell == null)
                    cell=NpoiRow.CreateCell(columnIndex);
                return cell.GetAdapter();
            }
        }

        public IEnumerator<ICell> GetEnumerator()
        {
            foreach (NpoiCell npoiCell in NpoiRow)
            {
                yield return npoiCell.GetAdapter();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object GetOriginal()
        {
            return NpoiRow;
        }
    }
}