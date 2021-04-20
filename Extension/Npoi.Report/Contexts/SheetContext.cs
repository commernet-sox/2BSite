using Npoi.Report.Accumulations;
using Npoi.Report.Driver;
using Npoi.Report.Extends;
using Npoi.Report.Meta;
using System;
using System.Collections.Generic;
namespace Npoi.Report.Contexts
{
    public sealed class SheetContext
    {
        private readonly RowIndexAccumulation _rowIndexAccumulation = new RowIndexAccumulation();
        private readonly ISheet _sheet;

        internal RowIndexAccumulation RowIndexAccumulation
        {
            get
            {
                return _rowIndexAccumulation;
            }
        }


        internal ISheet Sheet
        {
            get
            {
                return _sheet;
            }
        }

        public SheetContainer WorksheetContainer { get; }

        public bool IsEmpty()
        {
            return _sheet.IsNull();
        }

        public SheetContext(ISheet sheet, SheetContainer worksheetContainer)
        {
            _sheet = sheet;
            WorksheetContainer = worksheetContainer;
        }

        public ICell GetCell(Location location)
        {
            var rowIndex = _rowIndexAccumulation.GetCurrentRowIndex(location.RowIndex);

            IRow row = _sheet[rowIndex];
            if (row.IsNull())
            {
                return null;
            }
           
            return row[location.ColumnIndex];
        }

        public void CopyRepeaterTemplate(Repeater repeater, Action processTemplate)
        {

            if (_sheet is Npoi.Report.Driver.CSVDriver.Sheet)
            {
                var startRowIndex = _rowIndexAccumulation.GetCurrentRowIndex(repeater.Start.RowIndex);
                var endRowIndex = _rowIndexAccumulation.GetCurrentRowIndex(repeater.End.RowIndex);
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                int span = _sheet.CopyRows(startRowIndex, endRowIndex);
                sw.Stop();
                System.Diagnostics.Debug.WriteLine($"CopyRows:{ sw.ElapsedMilliseconds}ms");
                sw.Restart();
                ICell startCell = GetCell(repeater.Start);
                startCell.Value = startCell.GetStringValue().Replace($"<[{repeater.Name}]", String.Empty);
                processTemplate();
                ICell endCell = GetCell(repeater.End);
                endCell.Value = endCell.GetStringValue().Replace($">[{repeater.Name}]", String.Empty);
                sw.Stop();
                System.Diagnostics.Debug.WriteLine($"processTemplate:{ sw.ElapsedMilliseconds}ms");
                _rowIndexAccumulation.Add(span);
            }
            else if (_sheet is Npoi.Report.Driver.CSVDriver.Sheet)
            {
            }

        }

        public void RemoveRepeaterTemplate(Repeater repeater)
        {
            var startRowIndex = _rowIndexAccumulation.GetCurrentRowIndex(repeater.Start.RowIndex);
            var endRowIndex = _rowIndexAccumulation.GetCurrentRowIndex(repeater.End.RowIndex);

            int span = _sheet.RemoveRows(startRowIndex, endRowIndex);
            _rowIndexAccumulation.Add(-span);
        }
    }
}