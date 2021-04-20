namespace Npoi.Report.Driver
{
    public interface ICell
    {
        int RowIndex { get; }

        int ColumnIndex { get; }

        object Value { get; set; }
    }
}