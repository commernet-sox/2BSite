using NpoiCell = NPOI.SS.UserModel.ICell;

namespace Npoi.Report.Driver.NpoiDriver
{
    internal static class CellExtend
    {
        public static Cell GetAdapter(this NpoiCell cell)
        {
            if (null == cell)
            {
                return null;
            }
            return new Cell(cell);
        }
    }
}