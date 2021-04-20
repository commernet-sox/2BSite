using Npoi.Report.Contexts;

namespace Npoi.Report.Renderers
{
    public interface IEmbeddedRenderer<TSource>
    {
        void Render(SheetContext sheetContext, TSource dataSource);
    }
}