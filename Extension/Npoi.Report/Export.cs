using Npoi.Report.Contexts;
using Npoi.Report.Driver;
using Npoi.Report.Renderers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ExcelReport
{
    public sealed class Export
    {
        public static byte[] ExportToBuffer(string templateFile, params SheetRenderer[] sheetRenderers)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var str = Path.GetExtension(templateFile);
            IWorkbookLoader workbookLoader = Configurator.Get(str);
            IWorkbook workbook = workbookLoader.Load(templateFile);
            var workbookContext = new WorkbookContext(workbook);
            foreach (SheetRenderer sheetRenderer in sheetRenderers)
            {
                sheetRenderer.Render(workbookContext);
            }
            return workbook.SaveToBuffer();
            //List<Task> tasks = new List<Task>();
            //foreach (SheetRenderer sheetRenderer in sheetRenderers)
            //{
            //    tasks.Add(Task.Run(() =>
            //    {
            //        sheetRenderer.Render(workbookContext);
            //    }));

            //}
            //if (!Task.WaitAll(tasks.ToArray(), 600000))
            //{
            //    sw.Stop();
            //    System.Diagnostics.Debug.WriteLine($"总耗时:{ sw.ElapsedMilliseconds}ms");
            //    return null;
            //}
            //else
            //{
            //    sw.Stop();
            //    System.Diagnostics.Debug.WriteLine($"总耗时:{ sw.ElapsedMilliseconds}ms");
            //    return workbook.SaveToBuffer();
            //}
        }

        public static IWorkbook ExportToWorkbook(string templateFile, params SheetRenderer[] sheetRenderers)
        {
            var str = Path.GetExtension(templateFile);
            IWorkbookLoader workbookLoader = Configurator.Get(str);
            IWorkbook workbook = workbookLoader.Load(templateFile);
            var workbookContext = new WorkbookContext(workbook);
            foreach (SheetRenderer sheetRenderer in sheetRenderers)
            {
                sheetRenderer.Render(workbookContext);
            }
            return workbook;
        }
    }
}