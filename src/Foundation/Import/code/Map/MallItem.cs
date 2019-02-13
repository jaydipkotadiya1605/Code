using Sitecore.Data;

namespace Sitecore.Foundation.Import.Map
{
    public class MallItem
    {
        public ID MallID { get; set; }

        public string MallName { get; set; }

        public ID StoreRootID { get; set; }

        public ID BannerRootID { get; set; }

        public ID EventRootID { get; set; }

        public ID ArticleRootID { get; set; }

        public ID BlogRootID { get; set; }

        public ID PageRootID { get; set; }
    }
}