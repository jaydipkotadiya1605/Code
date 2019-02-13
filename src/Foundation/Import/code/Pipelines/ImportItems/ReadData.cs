using Sitecore.Diagnostics;
using System;

namespace Sitecore.Foundation.Import.Pipelines.ImportItems
{
    public class ReadData : ImportItemsProcessor
    {
        public override void Process(ImportItemsArgs args)
        {
            DataReaders.IDataReader reader;
            if (args.FileExtension.Equals(FileExtension.xlsx.ToString(), StringComparison.OrdinalIgnoreCase) ||
                     args.FileExtension.Equals(FileExtension.xls.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                reader = new DataReaders.XlsxDataReader();
            }
            else
            {
                Log.Info("Sitecore.Foundation.Import:Unsupported file format supplied. DataImporter accepts *.XLSX files",
                    this);
                return;
            }
            reader.ReadDataExtend(args);
            var count = 0;
            foreach (var importData in args.ImportDatas)
            {
                if(importData != null)
                    count += importData.Rows.Count;
            }
            args.Statistics.InputDataRows = count;
        }
    }
}