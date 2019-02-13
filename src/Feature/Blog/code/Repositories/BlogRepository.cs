namespace Sitecore.Feature.Blog.Repositories
{
    using Sitecore.Data.Items;
    using System.Linq;
    using Sitecore.Feature.Blog.Models;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Foundation.DependencyInjection;
    using FrasersContent = Foundation.FrasersContent;
    using Sitecore.Foundation.Search.Repositories;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Foundation.Abstractions.SitecoreContext;
    using System.Collections.Generic;
    using System;
    using Sitecore.Foundation.SitecoreExtensions.Repositories;
    using Sitecore.Foundation.Search.Models;

    [Service(typeof(IBlogRepository))]
    public class BlogRepository : IBlogRepository
    {
        public IBlogSearchRepository BlogSearchRepository { get; }
        private readonly IRenderingPropertiesRepository _renderingPropertiesRepository;
        private readonly ISitecoreContext _sitecoreContext;
        public BlogRepository(ISitecoreContext sitecoreContext, IRenderingPropertiesRepository renderingPropertiesRepository,
            IBlogSearchRepository BlogSearchRepository)
        {
            this._sitecoreContext = sitecoreContext;
            this.BlogSearchRepository = BlogSearchRepository;
            this._renderingPropertiesRepository = renderingPropertiesRepository;
        }

        public BlogItems GetBlogs(string category, int page = 0, int pageSize = 100)
        {
            var path = _sitecoreContext.Site.RootPath;
            var categories = new List<string>();
            if (!string.IsNullOrEmpty(category))
            {
                var id = GetCategoryId(category);
                if(!string.IsNullOrEmpty(id))
                    categories.Add(id);
            }

            var result = BlogSearchRepository.GetBlogItems(categories, page, pageSize, path);
            var hasMoreResult = pageSize < result.TotalNumberOfResults;

            return new BlogItems() {
                Items = result.ScoredItems.Select(x => CreateBlogItem(x.Item)).Where(x => x!= null).ToList(),
                HasMoreResult = hasMoreResult,
                PageSize = pageSize
            };
        }

        public BlogItems GetRelatedBlogs(Mvc.Presentation.Rendering rendering)
        {
            var pagingSetting = this._renderingPropertiesRepository.Get<PagingSettings>(rendering);
            var path = _sitecoreContext.Site.RootPath;
            var currentItem = _sitecoreContext.Item;
            var categories = GetCategoryCurrentItem(currentItem);

            var result = BlogSearchRepository.GetBlogItems(categories, FrasersContent.Constants.DedaultPage, pagingSetting.PageSize, path);

            return new BlogItems()
            {
                Items = result.ScoredItems.Select(x => CreateBlogItem(x.Item))
                                          .Where(x => x != null && x.Item.ID != currentItem.ID).ToList()
            };
        }

        private BlogItem CreateBlogItem(Item item)
        {
            if (item != null && item.IsDerived(FrasersContent.Templates.Blog.ID))
            {
                var thumbnailField = ((Data.Fields.ImageField)item.Fields[FrasersContent.Templates.Blog.Fields.Thumbnail]);
                string altText = thumbnailField.Alt;
                var blogItem =  new BlogItem
                {
                    Item = item,
                    Id = IdHelper.NormalizeGuid(item.ID),
                    Title = item.Fields[FrasersContent.Templates.Blog.Fields.Title].Value,
                    Summary = item.Fields[FrasersContent.Templates.Blog.Fields.Summary].Value,
                    Url = item.Url(),
                    Alt = string.IsNullOrEmpty(altText) ? item.Fields[FrasersContent.Templates.Blog.Fields.Title].Value : altText
                };
                if (item.FieldHasValue(FrasersContent.Templates.SchedulableSetting.Fields.PostDate))
                {
                    blogItem.PostDate = DateUtil.ToServerTime(item.GetDateTime(FrasersContent.Templates.SchedulableSetting.Fields.PostDate));
                }
                if (item.FieldHasValue(FrasersContent.Templates.SchedulableSetting.Fields.ExpiryDate))
                {
                    blogItem.ExpiryDate = DateUtil.ToServerTime(item.GetDateTime(FrasersContent.Templates.SchedulableSetting.Fields.ExpiryDate));
                }
                return blogItem;
              
            }
           
            return null;
        }

        private List<string> GetCategoryCurrentItem(Item currentItem)
        {
            var categories = new List<string>();
           
            if (currentItem != null && currentItem.IsDerived(FrasersContent.Templates.Blog.ID))
            {
                var categoryitems = currentItem.GetMultiListValueItems(FrasersContent.Templates.Blog.Fields.Category);
                if (categoryitems.Any())
                {
                    foreach (var item in categoryitems)
                    {
                        var id = IdHelper.NormalizeGuid(item.ID);
                        if (!string.IsNullOrEmpty(id))
                            categories.Add(id);
                    }
                }
            }
            return categories;
        }

        private string GetCategoryId(string categoryName)
        {
            if (!string.IsNullOrEmpty(categoryName))
            {
                var blogCategoryFolder = _sitecoreContext.Database.GetItem(FrasersContent.Templates.Blog.BlogCategoryFolder.ID);
                if (blogCategoryFolder != null)
                {
                    var item = blogCategoryFolder.GetChildren()
                            .FirstOrDefault(x => x.GetString(FrasersContent.Templates.Blog.BlogCategory.Fields.Key).ToLower().Equals(categoryName.ToLower()));
                    if (item != null)
                        return IdHelper.NormalizeGuid(item.ID);
                }
            }
            return null;
        }
    }
}