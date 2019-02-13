using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Shell;
using Sitecore.Shell.Applications.ContentEditor.Pipelines.RenderContentEditor;
using Sitecore.Text;
using Sitecore.Web.UI.HtmlControls;
using System.Text;

namespace Sitecore.Foundation.MediaInfo.Pipelines.RenderContentEditor
{
    /// <summary>
    /// The ShowMediaInfo class. 
    /// </summary>
    public class ShowMediaInfo
    {
        /// <summary>
        /// Gets a value indicating whether this section is collapsed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this section is collapsed; otherwise, <c>false</c>.
        /// </value>
        private static bool IsSectionCollapsed
        {
            get
            {
                var collapsedSections = new UrlString(Registry.GetString(Constants.CollapsedSetting));
                var value = collapsedSections[Constants.SectionName];
                return (string.IsNullOrEmpty(value) || (value == Constants.True));
            }
        }

        /// <summary>
        /// Processes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public void Process(RenderContentEditorArgs args)
        {
            var current = args.Item;
            if (current == null || !current.Template.FullName.StartsWith(Constants.TemplateFolder))
            {
                return;
            }

            var mediaItem = current;
            var renderMediaInfo = !IsSectionCollapsed || UserOptions.ContentEditor.RenderCollapsedSections;

            args.EditorFormatter.RenderSectionBegin(args.Parent,
                Constants.ControlId,
                Constants.SectionName,
                Constants.DisplayName,
                Constants.Icon,
                IsSectionCollapsed,
                UserOptions.ContentEditor.RenderCollapsedSections);

            if (renderMediaInfo)
            {
                RenderMediaInfo(args, mediaItem);
            }

            args.EditorFormatter.RenderSectionEnd(args.Parent, renderMediaInfo, true);
        }

        /// <summary>
        /// Renders the media info.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <param name="mediaItem">The media item.</param>
        private static void RenderMediaInfo(RenderContentEditorArgs args, MediaItem mediaItem)
        {
            var mediaUrlOptions = new MediaUrlOptions { AbsolutePath = false };

            var sb = new StringBuilder();
            sb.Append(Constants.DisplayFormat.OpenTable);

            // Get the path of the media item using it's path
            mediaUrlOptions.UseItemPath = true;
            sb.AppendFormat(Constants.DisplayFormat.Row, Constants.MediaInfo.UrlByPath, MediaManager.GetMediaUrl(mediaItem, mediaUrlOptions));

            // Get the path of the media item using it's ID
            mediaUrlOptions.UseItemPath = false;
            sb.AppendFormat(Constants.DisplayFormat.Row, Constants.MediaInfo.UrlById, MediaManager.GetMediaUrl(mediaItem, mediaUrlOptions));

            // Is it File or DB media?
            sb.AppendFormat(Constants.DisplayFormat.Row, Constants.MediaInfo.MediaLocation, mediaItem.FileBased ? Constants.MediaType.FileSystem : Constants.MediaType.Database);

            sb.Append(Constants.DisplayFormat.CloseTable);

            args.EditorFormatter.AddLiteralControl(args.Parent, sb.ToString());
        }
    }
}