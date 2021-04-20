namespace Npoi.Report.Meta
{
    public class WorkbookContainer
    {
        public Container<SheetContainer> Sheets { get; } = new Container<SheetContainer>();
    }
}