namespace Sitecore.Foundation.Search.Services
{
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Search.Models;
    using Sitecore.Foundation.SitecoreExtensions.Repositories;
    using Sitecore.Mvc.Presentation;

    [Service(typeof(IPagingService))]
    public class PagingService : IPagingService
    {
        private readonly IRenderingPropertiesRepository renderingPropertiesRepository;

        public PagingService(IRenderingPropertiesRepository renderingPropertiesRepository)
        {
            this.renderingPropertiesRepository = renderingPropertiesRepository;
        }

        public PagingSettings GetPagingSettings()
        {
            return this.renderingPropertiesRepository.Get<PagingSettings>(RenderingContext.Current.Rendering);
        }
    }
}