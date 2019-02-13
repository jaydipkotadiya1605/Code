using Sitecore.Foundation.Import.Configuration;
using Sitecore.Data.Fields;

namespace Sitecore.Foundation.Import.FieldUpdater
{
    public interface IFieldUpdater
    {
        void UpdateField(Field field, string importValue, IImportOptions importOptions);
    }
}
