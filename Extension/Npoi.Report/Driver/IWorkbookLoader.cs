namespace Npoi.Report.Driver
{
    public interface IWorkbookLoader
    {
        IWorkbook Load(string filePath);
    }
}