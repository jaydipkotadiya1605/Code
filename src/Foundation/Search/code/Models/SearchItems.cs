namespace Sitecore.Foundation.Search.Models
{
    using System.Collections.Generic;

    public class SearchItems
    {
        public SearchItems(IList<ScoredItem> scoredItems, int totalNumberOfResults)
        {
            this.ScoredItems = scoredItems;
            this.TotalNumberOfResults = totalNumberOfResults;
        }

        public IList<ScoredItem> ScoredItems { get; }

        public int TotalNumberOfResults { get; }
    }
}