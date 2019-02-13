using System.Data;

namespace Sitecore.Foundation.Import.Map
{
    public class TableInfo
    {
        public DataTable Datatable { get; set; }

        public int CurrentRow { get; set; }
    }
}