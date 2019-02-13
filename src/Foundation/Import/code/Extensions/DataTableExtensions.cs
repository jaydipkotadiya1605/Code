﻿using System.Data;
using System.Linq;

namespace Sitecore.Foundation.Import.Extensions
{
    public static class DataTableExtensions
    {
        public static DataTable GroupBy(this DataTable inputDataTable, string[] columnNames)
        {
            var groupedDataTable = new DataTable();
            var distinctColumnNames = columnNames.Distinct().ToArray();
            foreach (var columnName in distinctColumnNames)
            {
                groupedDataTable.Columns.Add(columnName, inputDataTable.Columns[columnName].DataType);
            }
            foreach (
                var grouping in
                    inputDataTable.AsEnumerable()
                        .GroupBy(r => new NTuple<object>(distinctColumnNames.Select(cn => r[cn]))))
            {
                var row = groupedDataTable.NewRow();
                for (int i = 0; i < distinctColumnNames.Length; i++)
                {
                    row[i] = grouping.Key.Values[i];
                }
                groupedDataTable.Rows.Add(row);
            }
            return groupedDataTable;
        }
    }
}