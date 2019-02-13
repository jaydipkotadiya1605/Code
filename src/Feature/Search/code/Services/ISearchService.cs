namespace Sitecore.Feature.Search.Services
{
    using Sitecore.Feature.Search.ViewModels;
    using Sitecore.Feature.Search.Models;

    public interface ISearchService
    {
        FraserRewardsViewModel GetLatestFraserRewards(FraserRewardsFilterModel criteria);

        SearchHeaderViewModel GetSearchHeader(Mvc.Presentation.Rendering rendering);

        SearchHeaderViewModel GetSearchOverlay(Mvc.Presentation.Rendering rendering);

        FraserRewardsViewModel Search(FraserRewardsFilterModel criteria);
    }
}