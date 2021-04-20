using NpoiSheet = NPOI.SS.UserModel.ISheet;

namespace Npoi.Report.Driver.NpoiDriver
{
    internal static class SheetExtend
    {
        public static Sheet GetAdapter(this NpoiSheet sheet)
        {
            if (null == sheet)
            {
                return null;
            }
            return new Sheet(sheet);
        }
    }
}