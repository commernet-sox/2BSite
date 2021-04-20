using Npoi.Report.Contexts;
using Npoi.Report.Extends;
using Npoi.Report.Meta;
using System.Collections.Generic;

namespace Npoi.Report.Renderers
{
    public sealed class SheetRenderer : Named
    {
        private IList<IElementRenderer> RendererList { set; get; }

        public SheetRenderer(string sheetName, params IElementRenderer[] elementRenderers)
        {
            Name = sheetName;
            RendererList = new List<IElementRenderer>(elementRenderers);
        }

        public void Render(WorkbookContext workbookContext)
        {
            SheetContext worksheetContext;
            lock (workbookContext)
            {
                worksheetContext = workbookContext[Name];
                if (worksheetContext.IsNull() || worksheetContext.IsEmpty())
                {
                    return;
                }
            }
            foreach (var renderer in RendererList)
            {
                renderer.Render(worksheetContext);
            }
        }
    }
}