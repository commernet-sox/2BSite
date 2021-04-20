using Npoi.Report.Contexts;
using Npoi.Report.Exceptions;
using Npoi.Report.Extends;
using Npoi.Report.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Npoi.Report.Renderers
{
    public class RepeaterRenderer<TItem> : Named, IElementRenderer
    {
        protected IEnumerable<TItem> DataSource { set; get; }

        protected IList<IEmbeddedRenderer<TItem>> RendererList { set; get; }



        public RepeaterRenderer(string name, IEnumerable<TItem> dataSource, params IEmbeddedRenderer<TItem>[] renderers)
        {
            Name = name;
            DataSource = dataSource;
            RendererList = new List<IEmbeddedRenderer<TItem>>(renderers);
        }

        public void Render(SheetContext sheetContext)
        {
            Repeater repeater = sheetContext.WorksheetContainer.Repeaters[Name];
            if (RendererList.IsNullOrEmpty())
            {
                throw new RenderException($"RepeaterRenderer[{repeater.Name}] is empty");
            }
            if (sheetContext.Sheet is Npoi.Report.Driver.NpoiDriver.Sheet)
            {
                var startRowIndex = repeater.Start.RowIndex;
                var sheet = sheetContext.Sheet as Npoi.Report.Driver.NpoiDriver.Sheet;
                int repeatRows = DataSource.Count();
                var listdatasource = DataSource.ToList();
                var rows = sheet.InsertRows(startRowIndex, DataSource.Count());
                foreach (var item in DataSource)
                {
              
                    foreach (var renderer in RendererList)
                    {
                        renderer.Render(sheetContext, item);
                    }
                    sheetContext.RowIndexAccumulation.Add(1);
                }

            }
            else if (sheetContext.Sheet is Npoi.Report.Driver.CSVDriver.Sheet)
            {
                foreach (var item in DataSource)
                {
                    sheetContext.CopyRepeaterTemplate(repeater, () =>
                    {
                        foreach (var renderer in RendererList)
                        {
                            renderer.Render(sheetContext, item);
                        }
                    });
                }
            }
            sheetContext.RemoveRepeaterTemplate(repeater);
        }
    }

    public class RepeaterRenderer<TSource, TItem> : Named, IEmbeddedRenderer<TSource>
    {
        protected Func<TSource, IEnumerable<TItem>> DgSetDataSource { set; get; }

        protected IList<IEmbeddedRenderer<TItem>> RendererList { set; get; }

        public RepeaterRenderer(string name, Func<TSource, IEnumerable<TItem>> dgSetDataSource, params IEmbeddedRenderer<TItem>[] renderers)
        {
            Name = name;
            DgSetDataSource = dgSetDataSource;
            RendererList = new List<IEmbeddedRenderer<TItem>>(renderers);
        }

        public void Render(SheetContext sheetContext, TSource dataSource)
        {
            Repeater repeater = sheetContext.WorksheetContainer.Repeaters[Name];
            if (RendererList.IsNullOrEmpty())
            {
                throw new RenderException($"RepeaterRenderer[{repeater.Name}] is empty");
            }

            foreach (var item in DgSetDataSource(dataSource))
            {
                sheetContext.CopyRepeaterTemplate(repeater, () =>
                {
                    foreach (var renderer in RendererList)
                    {
                        renderer.Render(sheetContext, item);
                    }
                });
            }
            sheetContext.RemoveRepeaterTemplate(repeater);
        }

        public void Append(IEmbeddedRenderer<TItem> renderer)
        {
            RendererList.Add(renderer);
        }
    }
}