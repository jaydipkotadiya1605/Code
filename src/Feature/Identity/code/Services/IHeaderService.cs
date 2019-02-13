using Sitecore.Feature.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Identity.Services
{
    public interface IHeaderService
    {
        MenuItems GetMainMenus();
        SocialItems GetSocialIcons();
    }
}