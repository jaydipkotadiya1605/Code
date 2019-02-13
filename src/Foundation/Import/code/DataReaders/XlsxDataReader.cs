using Excel;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Import.Pipelines.ImportItems;
using System;
using System.Data;
using System.Linq;

namespace Sitecore.Foundation.Import.DataReaders
{
    public class XlsxDataReader : IDataReader
    {
        public string[] GetColumnNames(ImportItemsArgs args)
        {
            Log.Info("Sitecore.Foundation.Import:Reading column names from input XSLX file...", this);
            try
            {
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)

                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(args.FileStream);

                excelReader.IsFirstRowAsColumnNames = true; //assume first line is data, so we can read it
                if (!excelReader.IsValid)
                {
                    Log.Info("Sitecore.Foundation.Import:Invalid Excel file '" + excelReader.ExceptionMessage + "'", this);
                    return new string[] {};
                }
                DataSet result = excelReader.AsDataSet();
                if (result == null)
                {
                    Log.Info("Sitecore.Foundation.Import:No data could be retrieved from Excel file.", this);
                    return new string[] { };
                }
                if (result.Tables == null || result.Tables.Count == 0)
                {
                    Log.Info("Sitecore.Foundation.Import:No worksheets found in Excel file", this);
                    return new string[] {};
                }
                var readDataTable = result.Tables[0];
                return readDataTable.Columns
                    .Cast<DataColumn>()
                    .Select(c => c.ColumnName).ToArray();
            }
            catch (Exception ex)
            {
                Log.Error("Sitecore.Foundation.Import:" + ex.ToString(), this);
            }
            return new string[] { };
        }

        public void ReadDataExtend(ImportItemsArgs args)
        {
            Log.Info("Sitecore.Foundation.Import:Reading XSLX input data", this);
            try
            {
                IExcelDataReader excelReader;
                if (args.FileExtension.Equals(Constants.ExcelFileExtention, StringComparison.OrdinalIgnoreCase))
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(args.FileStream, ReadOption.Loose);
                }
                else
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(args.FileStream);
                }

                excelReader.IsFirstRowAsColumnNames = args.ImportOptions.FirstRowAsColumnNames;
                if (!excelReader.IsValid)
                {
                    Log.Error("Sitecore.Foundation.Import:Invalid Excel file '" + excelReader.ExceptionMessage + "'", this);
                    return;
                }
                ConvertDataToDatatable(excelReader, args);
            }
            catch (Exception ex)
            {
                Log.Error("Sitecore.Foundation.Import:" + ex.ToString(), this);
            }
        }

        private void ConvertDataToDatatable(IExcelDataReader excelReader, ImportItemsArgs args)
        {
            DataSet result = excelReader.AsDataSet();
            if (result == null)
            {
                Log.Error("Sitecore.Foundation.Import:No data could be retrieved from Excel file.", this);
                return;
            }
            if (result.Tables == null || result.Tables.Count == 0)
            {
                Log.Error("Sitecore.Foundation.Import:No worksheets found in Excel file", this);
                return;
            }
            DataTable readDataTable = null;
            DataTable readDataTable1 = null;
            if (args.ContentType == ContentType.Article.ToString())
            {
                readDataTable = ConvertArticleTable(result, out readDataTable1);
                if (readDataTable1 == null)
                    readDataTable1 = new DataTable();
            }
            else
                readDataTable = result.Tables[0];
            if (readDataTable == null)
                readDataTable = new DataTable();

            args.ImportDatas.Add(readDataTable);
            if(readDataTable1 != null)
                args.ImportDatas.Add(readDataTable1);
            
            var countRow = readDataTable.Rows.Count;
            countRow = readDataTable1 != null ? countRow + readDataTable1.Rows.Count : countRow;
            Log.Info(string.Format("Sitecore.Foundation.Import:{0} records read from input data.", countRow), this);
        }

        private DataTable ConvertArticleTable(DataSet result, out DataTable readDataTable1)
        {
            DataTable tableMerge = null;
            readDataTable1 = null;
            for (int i = 0; i < result.Tables.Count; i++)
            {
                if (result.Tables[i].TableName != Constants.SheetNameIsMall)
                {
                    var categoryColumn = new DataColumn(Constants.CategoryNameColumn);
                    var categoryName = result.Tables[i].TableName;
                    categoryColumn.Expression = "'" + categoryName + "'";
                    if (tableMerge == null)
                    {
                        tableMerge = result.Tables[0].Clone();
                        tableMerge.Columns.Add(Constants.CategoryNameColumn);
                    }
                    result.Tables[i].Columns.Add(categoryColumn);
                    if (categoryName == FrasersContent.Constants.SpecialEventsName)
                    {
                        readDataTable1 = result.Tables[i];
                    }
                    else
                        tableMerge.Merge(result.Tables[i]);
                }
            }

            return tableMerge;
        }
    }
}