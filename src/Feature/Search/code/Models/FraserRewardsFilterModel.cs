namespace Sitecore.Feature.Search.Models
{
    using Sitecore.Foundation.Search.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sitecore.ContentSearch.Utilities;

    public class FraserRewardsFilterModel : PagingSettings
    {
        public string Title { get; set; }

        public string NotFoundMessage { get; set; }

        public string Types { get; set; }

        public string IsCurrentSite { get; set; }

        public string RenderingId { get; set; }

        public string Hidden { get; set; }

        public bool IsHiddenResultIfNotFound => MainUtil.GetBool(this.Hidden, false);

        public bool IsFilterOnCurrentSite => MainUtil.GetBool(this.IsCurrentSite, false);

        public IEnumerable<string> SelectedTypes => 
            HttpUtility.UrlDecode(this.Types)?.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x=> IdHelper.NormalizeGuid(x));

        public string Keyword { get; set; }
    }
}