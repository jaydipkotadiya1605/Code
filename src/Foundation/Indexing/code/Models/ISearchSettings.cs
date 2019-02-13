namespace Sitecore.Foundation.Indexing.Models
{
    using Sitecore.Data;
    using System.Collections.Generic;

    public interface ISearchSettings : IQueryRoot
    {
        IEnumerable<ID> Templates { get; }
        bool MustHaveFormatter { get; }
    }
}