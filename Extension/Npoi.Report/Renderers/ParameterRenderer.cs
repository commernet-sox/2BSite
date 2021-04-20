using Npoi.Report.Contexts;
using Npoi.Report.Driver;
using Npoi.Report.Exceptions;
using Npoi.Report.Extends;
using Npoi.Report.Meta;
using System;

namespace Npoi.Report.Renderers
{
    public class ParameterRenderer : Named, IElementRenderer
    {
        protected object Value { set; get; }

        public ParameterRenderer(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public void Render(SheetContext sheetContext)
        {
            Parameter parameter = sheetContext.WorksheetContainer.Parameters[Name];
            foreach (var location in parameter.Locations)
            {
                ICell cell = sheetContext.GetCell(location);
                if (null == cell)
                {
                    throw new RenderException($"parameter[{parameter.Name}],cell[{location.RowIndex},{location.ColumnIndex}] is null");
                }
                var parameterName = $"$[{parameter.Name}]";
                if (parameterName.Equals(cell.GetStringValue().Trim()))
                {
                    cell.Value = Value;
                }
                else
                {
                    cell.Value = (cell.GetStringValue().Replace(parameterName, Value.CastTo<string>()));
                }
            }
        }

        public Parameter GetParameter(SheetContext sheetContext)
        {
            return sheetContext.WorksheetContainer.Parameters[Name];
        }
    }

    public class ParameterRenderer<TSource> : Named, IEmbeddedRenderer<TSource>
    {
        protected Func<TSource, object> DgSetValue { set; get; }

        public ParameterRenderer(string name, Func<TSource, object> dgSetValue)
        {
            Name = name;
            DgSetValue = dgSetValue;
        }

        public void Render(SheetContext sheetContext, TSource dataSource)
        {
            Parameter parameter = sheetContext.WorksheetContainer.Parameters[Name];
            foreach (var location in parameter.Locations)
            {
                ICell cell = sheetContext.GetCell(location);
                if (null == cell)
                {
                    throw new RenderException($"parameter[{parameter.Name}],cell[{location.RowIndex},{location.ColumnIndex}] is null");
                }
                cell.Value = DgSetValue(dataSource);
            }
        }
    }
}