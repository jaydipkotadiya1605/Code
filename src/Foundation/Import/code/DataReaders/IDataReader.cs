using Sitecore.Foundation.Import.Pipelines.ImportItems;

namespace Sitecore.Foundation.Import.DataReaders
{
    public interface IDataReader
    {
        string[] GetColumnNames(ImportItemsArgs args);
        void ReadDataExtend(ImportItemsArgs args);
    }
}
