using Sitecore.Data.Fields;
using Sitecore.Foundation.Import.Configuration;

namespace Sitecore.Foundation.Import.FieldUpdater
{
    public class TextFieldUpdater : IFieldUpdater
    {
        public void UpdateField(Field field, string importValue, IImportOptions importOptions)
        {
            field.Value = importValue;
        }
    }
}