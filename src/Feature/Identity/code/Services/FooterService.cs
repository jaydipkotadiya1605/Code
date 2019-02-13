namespace Sitecore.Feature.Identity.Services
{
    using Sitecore.Feature.Identity.Models;
    using Sitecore.Feature.Identity.ViewModelBuilder;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;

    [Service(typeof(IFooterService))]
    public class FooterService : IFooterService
    {
        private readonly ISitecoreContext sitecoreContext;
        private readonly IFooterViewModelBuilder footerViewModelBuilder;

        public FooterService(
            ISitecoreContext sitecoreContext,
            IFooterViewModelBuilder footerViewModelBuilder)
        {
            this.sitecoreContext = sitecoreContext;
            this.footerViewModelBuilder = footerViewModelBuilder;
        }

        public CopyrightModel GetCopyright()
        {
            var item = this.sitecoreContext.Item.GetAncestorOrSelfOfTemplate(Templates.Footer.ID) ??
                this.sitecoreContext.Site.GetContextItem(Templates.Footer.ID);
            return item != null ? this.footerViewModelBuilder.GetCopyrightModel(item) : null;
        }

        public FooterModel GetFooter()
        {
            var item = this.sitecoreContext.Site.GetContextItem(Templates.Footer.ID);
            return item != null ? this.footerViewModelBuilder.GetFooterModel(item) : null;
        }
    }
}