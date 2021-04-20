
using NPOI.Extend;

namespace Npoi.Report.Driver.NpoiDriver
{
    public class WorkbookLoader : IWorkbookLoader
    {
        public IWorkbook Load(string filePath)
        {
            return NPOIHelper.LoadWorkbook(filePath).GetAdapter();
        }
    }
}