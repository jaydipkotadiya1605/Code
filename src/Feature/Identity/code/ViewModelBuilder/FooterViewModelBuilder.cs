namespace Sitecore.Feature.Identity.ViewModelBuilder
{
    using System.Linq;
    using Sitecore.Data.Items;
    using Sitecore.Data.Fields;
    using Sitecore.Feature.Identity.Models;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.ExperienceExplorer.Business.Utilities.Extensions;

    [Service(typeof(IFooterViewModelBuilder))]
    public class FooterViewModelBuilder : IFooterViewModelBuilder
    {
        public CopyrightModel GetCopyrightModel(Item item)
        {
            return new CopyrightModel
            {
                Logo = item.Field(Templates.Footer.Copyright.Fields.Logo),
                Text = item.Field(Templates.Footer.Copyright.Fields.Text),
                Links = item.GetFieldAsLinkedItem(Templates.Footer.Copyright.Fields.Links)?
                    .Children
                    .Where(x => x.IsDerived(Templates.FooterLink.ID))
                    .Select(x => x.Field(Templates.FooterLink.Fields.Link))
                    .ToList()
            };
        }

        public FooterModel GetFooterModel(Item item)
        {
            return new FooterModel
            {
                Logo = item.Field(Templates.Footer.Fields.Logo),
                Menu = item.GetFieldAsLinkedItem(Templates.Footer.Fields.Menu),
                IncludeHeaderSocialIcons = item.Fields[Templates.Footer.Fields.IncludeHeaderSocialIcons] != null ? ((CheckboxField)item.Fields[Templates.Footer.Fields.IncludeHeaderSocialIcons]).Checked : false,
            };
        }
    }
}