using NPOI.Extend;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Npoi.Report.CustomDynamicColumns
{
    public static class CustomExport
    {
        public static List<DateTime> CreateDateList(DateTime beginDate, DateTime endDate)
        {
            var list = new List<DateTime>();
            if (beginDate > endDate)
            {
                return list;
            }

            for (var date = beginDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                list.Add(date);
            }

            return list;
        }
        public static Stream WriteExcel<TData, TColumn>(string sheetName, IList<TData> modelList, IEnumerable<TColumn> dynamicColumns, Func<TColumn, object> format = null) where TData : class
        {
            if(sheetName is null)
            {
                return new MemoryStream();
            }
            if(modelList is null)
            {
                return new MemoryStream();
            }
            if(dynamicColumns is null)
            {
                return new MemoryStream();
            }

            var wb = new HSSFWorkbook();
            ISheet sheet = wb.CreateSheet(sheetName);

            #region 表头样式
            ICellStyle titleStyle = wb.CreateCellStyle();
            // 设置背景色
            titleStyle.FillForegroundColor = HSSFColor.Lime.Index;
            titleStyle.FillPattern = FillPattern.SolidForeground;
            // 让单元格居中
            titleStyle.Alignment = HorizontalAlignment.Center;
            // 上下居中
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.WrapText = true;//设置自动换行
                                       // 边框
            titleStyle.BorderBottom = BorderStyle.Thin;
            titleStyle.BorderLeft = BorderStyle.Thin;
            titleStyle.BorderRight = BorderStyle.Thin;
            titleStyle.BorderTop = BorderStyle.Thin;

            IFont titleFont = wb.CreateFont();
            titleFont.FontName = "宋体";
            // 设置字体大小
            titleFont.FontHeightInPoints = 12;
            // 加粗
            titleFont.Boldweight = (short)FontBoldWeight.Bold;
            titleStyle.SetFont(titleFont);
            #endregion

            #region 表体样式
            ICellStyle bodyStyle = wb.CreateCellStyle();
            // 让单元格居中
            bodyStyle.Alignment = HorizontalAlignment.Center;
            // 上下居中
            bodyStyle.VerticalAlignment = VerticalAlignment.Center;

            bodyStyle.BorderBottom = BorderStyle.Thin;
            bodyStyle.BorderLeft = BorderStyle.Thin;
            bodyStyle.BorderRight = BorderStyle.Thin;
            bodyStyle.BorderTop = BorderStyle.Thin;
            #endregion

            TData asTitleModel = FillingData<TData, TColumn>(sheet, bodyStyle, modelList, dynamicColumns, wb);

            var count = FillingTitle<TData, TColumn>(sheet, titleStyle, asTitleModel, dynamicColumns, format);

            for (int i=0;i<count;i++)
            {
                sheet.AutoSizeColumn(i, true);
            }

            var stream = new MemoryStream();
            wb.Write(stream);
            stream.Position = 0;

            return stream;
        }

        public static Stream WriteExcel<TData>(string sheetName, IList<TData> modelList) where TData : class
        {
            if (sheetName is null)
            {
                return new MemoryStream();
            }
            if (modelList is null)
            {
                return new MemoryStream();
            }
            

            var wb = new HSSFWorkbook();
            ISheet sheet = wb.CreateSheet(sheetName);

            #region 表头样式
            ICellStyle titleStyle = wb.CreateCellStyle();
            // 设置背景色
            titleStyle.FillForegroundColor = HSSFColor.Lime.Index;
            titleStyle.FillPattern = FillPattern.SolidForeground;
            // 让单元格居中
            titleStyle.Alignment = HorizontalAlignment.Center;
            // 上下居中
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.WrapText = true;//设置自动换行
                                       // 边框
            titleStyle.BorderBottom = BorderStyle.Thin;
            titleStyle.BorderLeft = BorderStyle.Thin;
            titleStyle.BorderRight = BorderStyle.Thin;
            titleStyle.BorderTop = BorderStyle.Thin;

            IFont titleFont = wb.CreateFont();
            titleFont.FontName = "宋体";
            // 设置字体大小
            titleFont.FontHeightInPoints = 12;
            // 加粗
            titleFont.Boldweight = (short)FontBoldWeight.Bold;
            titleStyle.SetFont(titleFont);
            #endregion

            #region 表体样式
            ICellStyle bodyStyle = wb.CreateCellStyle();
            // 让单元格居中
            bodyStyle.Alignment = HorizontalAlignment.Center;
            // 上下居中
            bodyStyle.VerticalAlignment = VerticalAlignment.Center;

            bodyStyle.BorderBottom = BorderStyle.Thin;
            bodyStyle.BorderLeft = BorderStyle.Thin;
            bodyStyle.BorderRight = BorderStyle.Thin;
            bodyStyle.BorderTop = BorderStyle.Thin;
            #endregion

            TData asTitleModel = FillingData<TData>(sheet, modelList, wb);

            var count = FillingTitle<TData>(sheet, titleStyle, asTitleModel);

            for (int i = 0; i < count; i++)
            {
                sheet.AutoSizeColumn(i, true);
            }

            var stream = new MemoryStream();
            wb.Write(stream);
            stream.Position = 0;

            return stream;
        }
        private static TData FillingData<TData, TColumn>(ISheet sheet, ICellStyle style, IList<TData> modelList,  IEnumerable<TColumn> dynamicColumns, IWorkbook workbook) where TData : class
        {

            TData asTitleModel = null;
            int maxSize = 0;
            Type type = typeof(TData);
            var properties = type.GetProperties().Where(p => p.GetCustomAttribute<TitleAttribute>() != null && !Attribute.IsDefined(p, typeof(IgnoreAttribute))).ToArray();

            IDataFormat dataformat = workbook.CreateDataFormat();
            var columnStyles = properties.Where(x =>x.PropertyType == typeof(decimal) || x.PropertyType == typeof(double))
                                     .ToDictionary(x => x.Name, x =>
                                        {
                                            var temp = workbook.CreateCellStyle();
                                            temp.CloneStyleFrom(style);
                                            if (x.PropertyType == typeof(decimal))
                                            {
                                                temp.Alignment = HorizontalAlignment.Right;
                                                temp.VerticalAlignment = VerticalAlignment.Center;
                                                temp.DataFormat = dataformat.GetFormat("0.0000");
                                            }
                                            else if( x.PropertyType == typeof(double))
                                            {
                                                temp.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                                            }

                                            return temp;
                                        });


            PropertyInfo dynamicColumnsProperty = GetDynamicColumnProperty(properties);
            if (dynamicColumnsProperty is null)
            {
                return null;
            }

            var types = dynamicColumnsProperty.PropertyType.GetGenericArguments();
            var subObjProperties = types[1].GetProperties().Where(p => p.GetCustomAttribute<TitleAttribute>() != null && !Attribute.IsDefined(p, typeof(IgnoreAttribute))).ToArray();
            var subColumnStyles = subObjProperties.Where(x => x.PropertyType == typeof(decimal) || x.PropertyType == typeof(double))
                                     .ToDictionary(x => x.Name, x =>
                                     {
                                         var temp = workbook.CreateCellStyle();
                                         temp.CloneStyleFrom(style);
                                         if (x.PropertyType == typeof(decimal))
                                         {
                                             temp.Alignment = HorizontalAlignment.Right;
                                             temp.VerticalAlignment = VerticalAlignment.Center;
                                             temp.DataFormat = dataformat.GetFormat("0.0000");
                                         }
                                         else if (x.PropertyType == typeof(double))
                                         {
                                             temp.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                                         }

                                         return temp;
                                     });


            for (int n = 0; n < modelList.Count; n++)
            {
                var aggregateModel = modelList[n];

                var dynamicColumnsObj = dynamicColumnsProperty.GetValue(aggregateModel);

                IDictionary dict;
                if (typeof(IDictionary).IsAssignableFrom(dynamicColumnsProperty.PropertyType)) // object
                {
                    dict = dynamicColumnsObj as IDictionary;
                }
                else
                {
                    return null;
                }

                int count = dict.Count;
                if (count > maxSize)
                {
                    maxSize = count;
                    asTitleModel = aggregateModel;
                }

                // 第零行为表头行，不填充数据
                IRow newRow = sheet.CreateRow(n + 1);
                int index = 0;

                foreach (var p in properties)
                {
                    var customStyle = columnStyles.ContainsKey(p.Name) ? columnStyles[p.Name] : null;

                    if (p.PropertyType == dynamicColumnsProperty.PropertyType)
                    {
                        var dataDict = dict;

                        foreach (var key in dynamicColumns)
                        {
                            foreach (var sp in subObjProperties)
                            {
                                var subCustomStyle = subColumnStyles.ContainsKey(sp.Name) ? subColumnStyles[sp.Name] : null;

                                ICell newCell = newRow.CreateCell(index);
                                newCell.SetCellType(CellType.Numeric);
                                if (subCustomStyle == null)
                                {
                                    newCell.CellStyle = style;
                                }
                                else
                                {
                                    newCell.CellStyle = subCustomStyle;
                                }

                                if (dataDict.Contains(key))
                                {
                                    var val = sp.GetValue(dataDict[key]);

                                    newCell.SetValue(val);
                                }
                                else
                                {
                                    newCell.SetCellValue("");
                                }

                                index++;
                            }
                        }
                    }
                    else
                    {
                        ICell newCell = newRow.CreateCell(index);
                        newCell.SetCellType(CellType.String);
                        if (customStyle == null)
                        {
                            newCell.CellStyle = style;
                        }
                        else
                        {
                            newCell.CellStyle = customStyle;
                        }

                        var val = p.GetValue(aggregateModel);

                        newCell.SetValue(val);

                        index++;
                    }
                }
            }

            return asTitleModel;
        }

        private static TData FillingData<TData>(ISheet sheet,  IList<TData> modelList, IWorkbook workbook) where TData : class
        {

            TData asTitleModel = null;

            Type type = typeof(TData);
            var properties = type.GetProperties().Where(p => p.GetCustomAttribute<TitleAttribute>() != null && !Attribute.IsDefined(p, typeof(IgnoreAttribute))).ToArray();

            IDataFormat dataformat = workbook.CreateDataFormat();
            var columnStyles = properties.Where(x => x.PropertyType == typeof(decimal) || x.PropertyType == typeof(double))
                                     .ToDictionary(x => x.Name, x =>
                                     {
                                         var temp = workbook.CreateCellStyle();
                                          
                                         if (x.PropertyType == typeof(decimal))
                                         {
                                             temp.Alignment = HorizontalAlignment.Right;
                                             temp.VerticalAlignment = VerticalAlignment.Center;
                                             temp.DataFormat = dataformat.GetFormat("0.0000");
                                         }
                                         else if (x.PropertyType == typeof(double))
                                         {
                                             temp.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                                         }

                                         return temp;
                                     });




            for (int n = 0; n < modelList.Count; n++)
            {
                var aggregateModel = modelList[n];
                asTitleModel = aggregateModel;
                // 第零行为表头行，不填充数据
                IRow newRow = sheet.CreateRow(n + 1);
                int index = 0;

                #region 表体样式
                ICellStyle bodyStyle = workbook.CreateCellStyle();
                // 让单元格居中
                bodyStyle.Alignment = HorizontalAlignment.Center;
                // 上下居中
                bodyStyle.VerticalAlignment = VerticalAlignment.Center;

                bodyStyle.BorderBottom = BorderStyle.Thin;
                bodyStyle.BorderLeft = BorderStyle.Thin;
                bodyStyle.BorderRight = BorderStyle.Thin;
                bodyStyle.BorderTop = BorderStyle.Thin;
                #endregion
                

                foreach (var p in properties)
                {
                    var customStyle = columnStyles.ContainsKey(p.Name) ? columnStyles[p.Name] : null;

                    ICell newCell = newRow.CreateCell(index);
                    newCell.SetCellType(CellType.String);

                    var val = p.GetValue(aggregateModel);
                    if (val is DateTime)
                    {
                        val = val?.ToString();
                    }

                    if (customStyle == null)
                    {
                        if (p.Name == "EmployeeType" && val.ToString() != "正式工")
                        {

                            bodyStyle.FillForegroundColor = HSSFColor.Red.Index;
                            bodyStyle.FillPattern = FillPattern.SolidForeground;
                        }
                         
                        newCell.CellStyle = bodyStyle;
                    }
                    else
                    {
                        if (p.Name == "EmployeeType" && val.ToString() != "正式工")
                        {

                            customStyle.FillForegroundColor = HSSFColor.Red.Index;
                            customStyle.FillPattern = FillPattern.SolidForeground;
                        }
                         
                        
                        newCell.CellStyle = customStyle;
                    }

                    
                    newCell.SetValue(val);

                    index++;


                }
            }

            return asTitleModel;
        }


        private static int FillingTitle<TData, TColumn>(ISheet sheet, ICellStyle style, TData asTitleModel, IEnumerable<TColumn> dynamicColumns, Func<TColumn, object> format = null) where TData : class
        {
            if (asTitleModel == null)
            {
                return 0;
            }

            Type type = typeof(TData);
            var properties = type.GetProperties();
            PropertyInfo dynamicColumnsProperty = GetDynamicColumnProperty(properties);
            if (dynamicColumnsProperty is null)
            {
                return 0;
            }

            IRow row = sheet.CreateRow(0);
            sheet.ShiftRows(row.RowNum + 1, sheet.LastRowNum, 2);
            var secondRow = sheet.CreateRow(row.RowNum + 1);
            var thirdRow = sheet.CreateRow(row.RowNum + 2);

            int index = 0;

            var types = dynamicColumnsProperty.PropertyType.GetGenericArguments();

            var subtitleProperties = types[1].GetProperties();
            foreach (var p in properties)
            {
                TitleAttribute columnMeta = p.GetCustomAttribute<TitleAttribute>();
                bool ignore = Attribute.IsDefined(p, typeof(IgnoreAttribute));

                if (columnMeta == null || ignore)
                {
                    continue;
                }

                if (p.PropertyType == dynamicColumnsProperty.PropertyType)//String DateTime decimal 都不是原生类型
                {
                    int savedFirstIndex = index;
                    foreach (var key in dynamicColumns)
                    {
                        int savedIndex = index;
                        foreach (var sp in subtitleProperties)
                        {
                            var titleAttribute = sp.GetCustomAttribute<TitleAttribute>();
                            bool ignoreSub = Attribute.IsDefined(sp, typeof(IgnoreAttribute));
                            if (titleAttribute == null || ignoreSub)
                            {
                                continue;
                            }
                            ICell newCell = row.CreateCell(index);
                            newCell.SetCellType(CellType.String);
                            newCell.CellStyle = style;
                            newCell.SetCellValue(columnMeta.Name);

                            ICell secondRowNewCell = secondRow.CreateCell(index);
                            secondRowNewCell.SetCellType(CellType.String);
                            secondRowNewCell.CellStyle = style;

                            if(format is null)
                            {
                                if (typeof(TColumn).IsValueType)
                                {
                                    secondRowNewCell.SetCellValue(key.ToString());
                                }
                                else
                                {
                                    secondRowNewCell.SetCellValue(key.ToString());
                                }
                            }
                            else
                            {
                                var val = format(key);

                                secondRowNewCell.SetValue(val);
                            }

                            ICell thirdRowNewCell = thirdRow.CreateCell(index);
                            thirdRowNewCell.SetCellType(CellType.String);
                            thirdRowNewCell.CellStyle = style;
                            thirdRowNewCell.SetCellValue(titleAttribute.Name);

                            index++;
                        }

                        if (savedIndex != (index - 1))
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(secondRow.RowNum, secondRow.RowNum, savedIndex, index - 1));
                        }
                    }
                    if (savedFirstIndex != (index - 1))
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(row.RowNum, row.RowNum, savedFirstIndex, index - 1));
                    }
                }
                else
                {
                    ICell newCell = row.CreateCell(index);
                    newCell.SetCellType(CellType.String);
                    newCell.CellStyle = style;
                    newCell.SetCellValue(columnMeta.Name);

                    ICell secondRowNewCell = secondRow.CreateCell(index);
                    secondRowNewCell.SetCellType(CellType.String);
                    secondRowNewCell.CellStyle = style;

                    ICell thirdRowNewCell = thirdRow.CreateCell(index);
                    thirdRowNewCell.SetCellType(CellType.String);
                    thirdRowNewCell.CellStyle = style;

                    var region = new CellRangeAddress(row.RowNum, thirdRow.RowNum, index, index);
                    sheet.AddMergedRegion(region);
                    //((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, BorderStyle.Thin, NPOI.HSSF.Util.HSSFColor.Black.Index);

                    index++;
                }
            }

            return index;
        }

        private static int FillingTitle<TData>(ISheet sheet, ICellStyle style, TData asTitleModel) where TData : class
        {
            if (asTitleModel == null)
            {
                return 0;
            }

            Type type = typeof(TData);
            var properties = type.GetProperties();
          

            IRow row = sheet.CreateRow(0);
            sheet.ShiftRows(row.RowNum + 1, sheet.LastRowNum, 2);
            var secondRow = sheet.CreateRow(row.RowNum + 1);
            var thirdRow = sheet.CreateRow(row.RowNum + 2);

            int index = 0; 
            foreach (var p in properties)
            {
                TitleAttribute columnMeta = p.GetCustomAttribute<TitleAttribute>();
                bool ignore = Attribute.IsDefined(p, typeof(IgnoreAttribute));

                if (columnMeta == null || ignore)
                {
                    continue;
                }

                 
                    ICell newCell = row.CreateCell(index);
                    newCell.SetCellType(CellType.String);
                    newCell.CellStyle = style;
                    newCell.SetCellValue(columnMeta.Name);

                    ICell secondRowNewCell = secondRow.CreateCell(index);
                    secondRowNewCell.SetCellType(CellType.String);
                    secondRowNewCell.CellStyle = style;

                    ICell thirdRowNewCell = thirdRow.CreateCell(index);
                    thirdRowNewCell.SetCellType(CellType.String);
                    thirdRowNewCell.CellStyle = style;

                    var region = new CellRangeAddress(row.RowNum, thirdRow.RowNum, index, index);
                    sheet.AddMergedRegion(region);
                    //((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, BorderStyle.Thin, NPOI.HSSF.Util.HSSFColor.Black.Index);

                    index++;
                 
            }

            return index;
        }

        private static PropertyInfo GetDynamicColumnProperty(PropertyInfo[] properties)
        {
            foreach (var p in properties)
            {
                bool flag = p.PropertyType.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDictionary<,>));
                if (flag)
                {
                    return p;
                }
            }

            return null;
        }
    }

    public class ExcelOpener
    {
        //https://github.com/bit-twit/poi-shift-column/blob/master/src/test/java/org/bittwit/poi/ExcelOpenerTest.java
        private string sourceFile;
        private string outputFile;
        private XSSFWorkbook workbook;

        public ExcelOpener(string inputFile, string outputFile)
        {
            this.sourceFile = inputFile;
            this.outputFile = outputFile;
        }

        public void Open()
        {
            workbook = new XSSFWorkbook(new FileStream(this.sourceFile, FileMode.Open));

            WriteToFile(workbook, this.outputFile);

            workbook.Close();//打开原文件保存为输出文件，在输出文件上修改，保持原文件不变

            // open output file to edit
            workbook = new XSSFWorkbook(new FileStream(this.outputFile, FileMode.Open));

        }

        public int GetNumberOfRows(int sheetIndex)
        {
            System.Diagnostics.Debug.Assert(workbook != null);

            int sheetNumber = workbook.NumberOfSheets;

            Console.WriteLine("Found " + sheetNumber + " sheets.");

            if (sheetIndex >= sheetNumber)
            {
                throw new RuntimeException("Sheet index " + sheetIndex
                        + " invalid, we have " + sheetNumber + " sheets");
            }

            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            int rowNum = sheet.LastRowNum + 1;

            Console.WriteLine("Found " + rowNum + " rows.");

            return rowNum;
        }

        public void InsertNewColumnBefore(int sheetIndex, int columnIndex)
        {
            System.Diagnostics.Debug.Assert(workbook != null);

            IFormulaEvaluator evaluator = workbook.GetCreationHelper()
                                                  .CreateFormulaEvaluator();
            evaluator.ClearAllCachedResultValues();

            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            int nrRows = GetNumberOfRows(sheetIndex);
            int nrCols = GetNrColumns(sheetIndex);
            //System.out.println("Inserting new column at " + columnIndex);

            for (int row = 0; row < nrRows; row++)
            {
                IRow r = sheet.GetRow(row);

                if (r == null)
                {
                    continue;
                }

                // shift to right
                for (int col = nrCols; col > columnIndex; col--)
                {
                    ICell rightCell = r.GetCell(col);
                    if (rightCell != null)
                    {
                        r.RemoveCell(rightCell);
                    }

                    ICell leftCell = r.GetCell(col - 1);

                    if (leftCell != null)
                    {
                        ICell newCell = r.CreateCell(col, leftCell.CellType);
                        CloneCell(newCell, leftCell);
                        if (newCell.CellType == CellType.Formula)
                        {
                            newCell.CellFormula = ExcelHelperEX.UpdateFormula(newCell.CellFormula, columnIndex);
                            evaluator.NotifySetFormula(newCell);
                            CellValue cellValue = evaluator.Evaluate(newCell);
                            evaluator.EvaluateFormulaCell(newCell);

                            Console.WriteLine(cellValue);
                        }
                    }
                }

                // delete old column
                var cellType = CellType.Blank;

                ICell currentEmptyWeekCell = r.GetCell(columnIndex);
                if (currentEmptyWeekCell != null)
                {
                    //				cellType = currentEmptyWeekCell.getCellType();
                    r.RemoveCell(currentEmptyWeekCell);
                }

                // create new column
                r.CreateCell(columnIndex, cellType);
            }

            // Adjust the column widths
            for (int col = nrCols; col > columnIndex; col--)
            {
                sheet.SetColumnWidth(col, sheet.GetColumnWidth(col - 1));
            }

            // currently updates formula on the last cell of the moved column
            // TODO: update all cells if their formulas contain references to the moved cell
            //		Row specialRow = sheet.getRow(nrRows-1);
            //		Cell cellFormula = specialRow.createCell(nrCols - 1);
            //		cellFormula.setCellType(XSSFCell.CELL_TYPE_FORMULA);
            //		cellFormula.setCellFormula(formula);

            XSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
        }

        /*
		 * Takes an existing Cell and merges all the styles and forumla into the new
		 * one
		 */
        private static void CloneCell(ICell cNew, ICell cOld)
        {
            cNew.CellComment = cOld.CellComment;
            cNew.CellStyle = cOld.CellStyle;

            switch (cOld.CellType)
            {
                case CellType.Boolean:
                    {
                        cNew.SetCellValue(cOld.BooleanCellValue);
                        break;
                    }
                case CellType.Numeric:
                    {
                        cNew.SetCellValue(cOld.NumericCellValue);
                        break;
                    }
                case CellType.String:
                    {
                        cNew.SetCellValue(cOld.StringCellValue);
                        break;
                    }
                case CellType.Error:
                    {
                        cNew.SetCellValue(cOld.ErrorCellValue);
                        break;
                    }
                case CellType.Formula:
                    {
                        cNew.SetCellFormula(cOld.CellFormula);
                        break;
                    }
            }
        }


        void ShiftColumns(IRow row, int startingIndex, int shiftCount)
        {
            for (int i = row.PhysicalNumberOfCells - 1; i >= startingIndex; i--)
            {
                ICell oldCell = row.GetCell(i);
                ICell newCell = row.CreateCell(i + shiftCount, oldCell.CellType);
                CloneCellValue(oldCell, newCell);
            }
        }

        void CloneCellValue(ICell oldCell, ICell newCell)
        { //TODO test it
            switch (oldCell.CellType)
            {
                case CellType.String:
                    newCell.SetCellValue(oldCell.StringCellValue);
                    break;
                case CellType.Numeric:
                    newCell.SetCellValue(oldCell.NumericCellValue);
                    break;
                case CellType.Boolean:
                    newCell.SetCellValue(oldCell.BooleanCellValue);
                    break;
                case CellType.Formula:
                    newCell.SetCellFormula(oldCell.CellFormula);
                    break;
                case CellType.Error:
                    newCell.SetCellErrorValue(oldCell.ErrorCellValue);
                    break;
                case CellType.Blank:
                case CellType.Unknown:
                    break;
            }
        }

        public int GetNrColumns(int sheetIndex)
        {
            System.Diagnostics.Debug.Assert(workbook != null);

            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            // get header row
            IRow headerRow = sheet.GetRow(0);
            int nrCol = headerRow.LastCellNum;

            // while
            // (!headerRow.getCell(nrCol++).getStringCellValue().equals(LAST_COLUMN_HEADER));

            // while (nrCol <= headerRow.getPhysicalNumberOfCells()) {
            // Cell c = headerRow.getCell(nrCol);
            // nrCol++;
            //
            // if (c!= null && c.getCellType() == Cell.CELL_TYPE_STRING) {
            // if (c.getStringCellValue().equals(LAST_COLUMN_HEADER)) {
            // break;
            // }
            // }
            // }

            //System.out.println("Found " + nrCol + " columns.");
            return nrCol;

        }

        private static void WriteToFile(IWorkbook workbook, string fileName)
        {
            using (var fileOut = new FileStream(fileName, FileMode.CreateNew))
            {
                workbook.Write(fileOut);
                //workbook.Close();
            }
        }

        public void Close()
        {
            System.Diagnostics.Debug.Assert(workbook != null);

            WriteToFile(workbook, this.outputFile);
            workbook.Close();
        }

    }
    public class ExcelHelperEX
    {
        private static readonly char LETTERS_IN_EN_ALFABET = (char)26;
        private static readonly char BASE = LETTERS_IN_EN_ALFABET;
        private static readonly char A_LETTER = (char)65;

        /**
		 * Replaces occurences of the text representation of columnIndex to columnIndex+1.
		 * Ex:
		 * "B6:B8" (columnIndex = 2) -> "C6:C8" (columnIndex = 3)
		 *
		 *
		 * @param cellFormula
		 * @param columnIndex
		 * @return
		 */
        public static string UpdateFormula(String cellFormula, int columnIndex)
        {
            string existingColName = GetReferenceForColumnIndex(columnIndex);
            string newColName = GetReferenceForColumnIndex(columnIndex + 1);


            string newCellFormula = cellFormula.Replace(existingColName, newColName);

            Console.WriteLine("Replacing : " + existingColName + " with : " + newColName + " in "
                    + cellFormula + ", result: " + newCellFormula);
            return newCellFormula;
        }

        /**
		 * Does a "Base 26" - Base26E transformation on the given index, to obtain the alphabet representation.
		 * The transformation is not exactly Base26, since the factor for each degree of power (besides first)
		 * is represented as "1 less".
		 *
		 * Ex:
		 *  25 -> Z
		 *  26 -> BA (in Base26) -> AA (in Excel)
		 *  27 -> BB (in Base26) -> AB (in Excel)
		 *  (we have B instead of A for degree of power 1)
		 * So a normal 'AACAAA' in Base26 is 'BBDBBA' in Base26E.
		 * BACAA in Base26 is ZDBA in Base26E
		 *
		 * This is how excel identifies columns in formulas.
		 *
		 * @param columnIndex
		 * @return
		 */
        public static string GetReferenceForColumnIndex(int columnIndex)
        {
            StringBuilder sb = new StringBuilder();

            while (columnIndex >= 0)
            {
                if (columnIndex == 0)
                {
                    sb.Append((char)A_LETTER);
                    break;
                }

                char code = (char)(columnIndex % BASE);
                char letter = (char)(code + A_LETTER);
                sb.Append(letter);

                columnIndex /= BASE;
                columnIndex -= 1;
            }

            return new string(sb.ToString().ToCharArray().Reverse().ToArray());
        }


    }
}
