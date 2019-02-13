using Sitecore.Data.Fields;
using Sitecore.Foundation.Import.Configuration;
using System;

namespace Sitecore.Foundation.Import.FieldUpdater
{
    public class DatetimeUpdater : IFieldUpdater
    {
        public void UpdateField(Field field, string importValue, IImportOptions importOptions)
        {
            DateTime dateTime;
            if (DateTime.TryParse(importValue, out dateTime))
            {
                dateTime = DateTime.Parse(importValue);
                field.Value = DateUtil.ToIsoDate(dateTime);
            }
        }
    }
}