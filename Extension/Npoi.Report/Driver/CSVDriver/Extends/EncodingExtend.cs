using System.Text;

namespace Npoi.Report.Driver.CSVDriver.Extends
{
    internal static class EncodingExtend
    {
        static EncodingExtend()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static Encoding GB2312
        {
            get
            {
                return Encoding.GetEncoding("GB2312");
            }
        }
    }
}