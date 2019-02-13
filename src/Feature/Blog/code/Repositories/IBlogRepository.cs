using Sitecore.Feature.Blog.Models;

namespace Sitecore.Feature.Blog.Repositories
{
    public interface IBlogRepository
    {
        BlogItems GetBlogs(string category, int page = 0, int pageSize = 100);
        BlogItems GetRelatedBlogs(Mvc.Presentation.Rendering rendering);
    }
}
