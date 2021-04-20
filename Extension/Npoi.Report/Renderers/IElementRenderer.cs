using Npoi.Report.Contexts;

namespace Npoi.Report.Renderers
{
    public interface IElementRenderer
    {
        void Render(SheetContext sheetContext);
    }
}