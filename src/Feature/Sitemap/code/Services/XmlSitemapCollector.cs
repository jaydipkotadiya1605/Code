namespace Sitecore.Feature.Sitemap.Services
{
    using Sitecore.Configuration;
    using Sitecore.ContentSearch.Linq.Utilities;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Data.Items;
    using Sitecore.Feature.Sitemap.Models;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.ItemResolver.Extensions;
    using Sitecore.Foundation.ItemResolver.Providers.RouteResolving;
    using Sitecore.Foundation.Search.Models.Index;
    using Sitecore.Foundation.Search.Repositories;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Links;
    using Sitecore.Sites;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    [Service(typeof(IXmlSitemapCollector))]
    public class XmlSitemapCollector : IXmlSitemapCollector
    {
        private readonly IGlobalSearchRepository globalSearchRepository;

        public XmlSitemapCollector(
            IGlobalSearchRepository globalSearchRepository)
        {
            this.globalSearchRepository = globalSearchRepository;
        }

        public IEnumerable<SitemapModel> GetSitemapItems(SitemapDefinitionModel sitemapDefinition, SiteContext siteContext)
        {
            var sitemapitems = new List<SitemapModel>();
            var urlList = new List<string>();

            var embedLanguage = sitemapDefinition.EmbedLanguage;
            var db = Context.Database != null ? Context.Database : Factory.GetDatabase(Sitemap.Constants.WebDatabase);

            var urlOptions = UrlOptions.DefaultOptions;
            urlOptions.AlwaysIncludeServerUrl = true;

            urlOptions.LanguageEmbedding = embedLanguage ? LanguageEmbedding.Always : LanguageEmbedding.Never;

            var items = db.SelectItems($"fast:{siteContext.StartPath}//*")
                .Where(i => IsSitemapItem(i, sitemapDefinition))
                .ToList();

            var dynamicItems = this.GetDynamicWildcardItems(siteContext);
            items.AddRange(dynamicItems);

            var startItem = siteContext.GetStartItem();
            if (IsSitemapItem(startItem, sitemapDefinition))
            {
                items.Insert(0, startItem);
            }
            foreach (var item in items)
            {
                var regex = new Regex(@"(\/[A-Za-z0-9]\/)");
                var url = LinkManager.GetItemUrl(item, urlOptions);
                url = regex.Replace(url, "/").ToLowerInvariant();
                if (urlList.Contains(url)) continue;

                urlList.Add(url);
                sitemapitems.Add(new SitemapModel
                {
                    Url = url,
                    LastModified = item.Statistics.Updated
                });
            }
            return sitemapitems;
        }

        private static bool IsSitemapItem(Item item, SitemapDefinitionModel sitemapDefinition)
        {
            return item.HasVersion() &&
                   item.HasLayout() &&
                   (item.IsDerived(Templates.Sitemap.Id) ||
                    sitemapDefinition.IncludedTemplates.Contains(item.ID) ||
                    sitemapDefinition.IncludedBaseTemplates.Any(item.IsDerived)) &&
                   !sitemapDefinition.ExcludedItems.Contains(item.ID);
        }

        private List<Item> GetDynamicWildcardItems(SiteContext siteContext)
        {
            var itemResolverSettings = siteContext.ItemResolverSettings();
            var routes = itemResolverSettings.Children
                .Where(x => x.IsDerived(Foundation.ItemResolver.Templates.ItemResolverRule.ID))
                .Select(x => new WildcardRouteItem(x)).ToList();

            var dynamicItems = new List<Item>();

            routes.ForEach(route =>
            {
                var templateId = route.DataTemplate.ID;
                var routepath = route.SearchRootNode.Paths.Path;
                var query = PredicateBuilder.Create<SchedulableItemIndex>(x =>
                    x.AllTemplates.Contains(IdHelper.NormalizeGuid(templateId)) &&
                    x.AllTemplates.Contains(IdHelper.NormalizeGuid(Templates.Sitemap.Id)) &&
                    x.Path.StartsWith(routepath));

                if (route.DataTemplate.IsDerived(Foundation.FrasersContent.Templates.SchedulableContent.ID))
                {
                    var postDateQuery = PredicateBuilder.False<SchedulableItemIndex>();
                    postDateQuery.Or(x => x.PostDateHasValue && x.PostDate <= DateTime.UtcNow);
                    postDateQuery.Or(x => !x.PostDateHasValue);

                    var expiryDateQuery = PredicateBuilder.False<SchedulableItemIndex>();
                    expiryDateQuery.Or(x => x.ExpiryDateHasValue && x.ExpiryDate >= DateTime.UtcNow);
                    expiryDateQuery.Or(x => !x.ExpiryDateHasValue);

                    query = query.And(postDateQuery);
                    query = query.And(expiryDateQuery);
                }

                var results = this.globalSearchRepository.Search(query);
                dynamicItems.AddRange(results.ScoredItems.Where(x => x.Item != null).Select(x => x.Item));
            });

            return dynamicItems;
        }
    }
}