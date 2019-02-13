namespace Sitecore.Feature.Multisite.Services
{
    using Sitecore.Feature.Multisite.Models;
    using System.Collections.Generic;

    public interface IMultisiteService
    {
        IEnumerable<MallViewModel> GetAllMalls();
        SitesViewModel GetSites();
    }
}