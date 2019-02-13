namespace Sitecore.Foundation.Indexing
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct IndexedItem
        {
            public static ID ID => new ID("{8FD6C8B6-A9A4-4322-947E-90CE3D94916D}");

            public struct Fields
            {
                public const string IncludeInSearchResults_FieldName = "IncludeInSearchResults";
            }
        }
    }
}