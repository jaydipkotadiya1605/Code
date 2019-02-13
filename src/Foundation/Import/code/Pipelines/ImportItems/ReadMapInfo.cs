using Sitecore.Diagnostics;

namespace Sitecore.Foundation.Import.Pipelines.ImportItems
{
    public class ReadMapInfo : ImportItemsProcessor
    {
        public override void Process(ImportItemsArgs args)
        {
            Log.Info("Sitecore.Foundation.Import:Processing import map...", this);
            args.ImportDatas[0].Columns.Clear();
            foreach (var column in args.Map[0].InputFields)
            {
                args.ImportDatas[0].Columns.Add(column.Name, typeof(string));
            }
            Log.Info(string.Format("Sitecore.Foundation.Import:{0} Columns defined in map.", args.Map[0].InputFields.Count), this);
        }
    }
}