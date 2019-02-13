using Sitecore.Diagnostics;

namespace Sitecore.Foundation.Import.Pipelines.ImportItems
{
    public class ValidateArgs : ImportItemsProcessor
    {
        public override void Process(ImportItemsArgs args)
        {
            Log.Info("Sitecore.Foundation.Import:Validating input...", this);
            var argsValid = true;
            if (args.FileStream == null)
            {
                Log.Error("Sitecore.Foundation.Import:Input file not found.", this);
                argsValid = false;
            }
            if (!argsValid)
            {
                args.AddMessage("Error: Input file not found.");
                args.ErrorDetail = "FileStream = null";
                args.AbortPipeline();
            }
        }
    }
}