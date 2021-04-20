using NpoiWorkbook = NPOI.SS.UserModel.IWorkbook;

namespace Npoi.Report.Driver.NpoiDriver
{
    internal static class WorkbookExtend
    {
        public static Workbook GetAdapter(this NpoiWorkbook workbook)
        {
            if (null == workbook)
            {
                return null;
            }
            return new Workbook(workbook);
        }
    }
}