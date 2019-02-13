using Sitecore.Feature.Navigation.Models;
using Sitecore.Foundation.Abstractions.SitecoreContext;

namespace Sitecore.Feature.Navigation.Repositories
{
    public interface INavigationRepository
    {
        NavigationItems GetMainMenu();
        NavigationItems GetBreadcrumb();
        NavigationItems GetTabMenu(bool isGetSubTab);
        NavigationItems GetTabFilter(string categoryName, string datasource, string mallId = "");
        NavigationItems GetHorizontalTabPages();
    }
}
