namespace Sitecore.Foundation.Search.Models
{
    public class PagingSettings
    {
        private int pageIndex;
        private int pageSize;
        private const int DefaultResultsOnPage = 10;
        private const int DefaultPagesToShow = 1;

        public int PageIndex
        {
            get
            {
                return this.pageIndex < DefaultPagesToShow ? DefaultPagesToShow : this.pageIndex;
            }
            set
            {
                this.pageIndex = value;
            }
        }

        public int PageSize
        {
            get
            {
                return this.pageSize < 1 ? DefaultResultsOnPage : this.pageSize;
            }
            set
            {
                this.pageSize = value;
            }
        }
    }
}