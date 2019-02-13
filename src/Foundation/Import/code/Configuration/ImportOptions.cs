﻿namespace Sitecore.Foundation.Import.Configuration
{
    public class ImportOptions : IImportOptions
    {
        public InvalidLinkHandling InvalidLinkHandling { get; set; }

        public ExistingItemHandling ExistingItemHandling { get; set; }

        public string MultipleValuesImportSeparator { get; set; }

        public string TreePathValuesImportSeparator { get; set; }

        public string[] CsvDelimiter { get; set; }

        public bool FirstRowAsColumnNames { get; set; }
    }
}