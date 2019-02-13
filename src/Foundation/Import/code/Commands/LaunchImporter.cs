using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Foundation.Import.Commands
{
    public class LaunchImporter : Command
    {
        public override void Execute(CommandContext context)
        {
            string url = Sitecore.Configuration.Settings.GetSetting("Sitecore.Foundation.Import.ImporterDialog");
            SheerResponse.ShowModalDialog(new ModalDialogOptions(url)
            {
                Width = "340px",
                Height = "400px",
                Response = false,
                ForceDialogSize = true,
                Maximizable = true
            });
        }
    }
}