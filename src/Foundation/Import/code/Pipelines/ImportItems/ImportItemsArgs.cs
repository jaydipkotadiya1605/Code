using Sitecore.Foundation.Import.Configuration;
using Sitecore.Foundation.Import.Map;
using Sitecore.Data;
using Sitecore.Globalization;
using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace Sitecore.Foundation.Import.Pipelines.ImportItems
{
    [Serializable]
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class ImportItemsArgs : PipelineArgs
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public string FileExtension { get; set; }
        public Stream FileStream { get; set; }
        public Database Database { get; set; }
        public string ContentType { get; set; }
        public ID RootItemId { get; set; }
        public Language TargetLanguage { get; set; }
        public List<ItemImportMap> Map { get; set; }
        public IImportOptions ImportOptions { get; set; }
        public ImportStatistics Statistics { get; set; }
        public List<DataTable> ImportDatas { get; set; }
        public List<ItemDto> ImportItems { get; set; }
        public string ErrorDetail { get; set; }
        public Stopwatch Timer { get; set; }
        public List<MallItem> MallItems { get; set; }
        public string CurrentImportMallIdTemp { get; set; }
        public ImportItemsArgs()
        {
            Statistics = new ImportStatistics();
            ImportDatas = new List<DataTable>();
            ImportItems = new List<ItemDto>();
            Map = new List<ItemImportMap>();
            Timer = new Stopwatch();
        }

        protected ImportItemsArgs(SerializationInfo info, StreamingContext context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotSupportedException();
        }
    }
}
