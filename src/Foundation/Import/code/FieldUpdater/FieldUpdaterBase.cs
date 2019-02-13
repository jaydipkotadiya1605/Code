using System;
using Sitecore.Foundation.Import.Configuration;

namespace Sitecore.Foundation.Import.FieldUpdater
{
    public abstract class FieldUpdaterBase : IFieldUpdater
    {
        public void UpdateField(Sitecore.Data.Fields.Field field, string importValue, IImportOptions importOptions)
        {
            throw new NotImplementedException();
        }
    }
}