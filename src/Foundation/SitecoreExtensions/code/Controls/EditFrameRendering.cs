namespace Sitecore.Foundation.SitecoreExtensions.Controls
{
    using Sitecore.Web.UI.WebControls;
    using System;
    using System.IO;
    using System.Web.UI;

    /// <summary>
    ///   Edit frame class.
    /// </summary>
    /// <remarks>
    ///   This class is required because MVC doesn't support the EditFrame control.
    /// </remarks>
    /// <see cref="HtmlHelperExtensions.BeginEditFrame{T}"/>
    public sealed class EditFrameRendering : IDisposable
    {
        private readonly EditFrame editFrame;
        private readonly HtmlTextWriter htmlWriter;
       
        public EditFrameRendering(TextWriter writer, string dataSource, string buttons)
        {
            this.htmlWriter = new HtmlTextWriter(writer);
            this.editFrame = new EditFrame
            {
                DataSource = dataSource,
                Buttons = buttons
            };
            this.editFrame.RenderFirstPart(this.htmlWriter);
        }

        public void Dispose()
        {
            this.editFrame.RenderLastPart(this.htmlWriter);
            this.htmlWriter.Dispose();
        }
    }
}