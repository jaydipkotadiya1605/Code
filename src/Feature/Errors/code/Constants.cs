namespace Sitecore.Feature.Errors
{
    using System.Collections.Generic;

    public static class Constants
    {
        public static readonly IEnumerable<string> ExcludedPaths = new List<string>
        {
            @"/sitecore",
            @"/api",
            @"/styles",
            @"/js",
            @"/fonts",
            @"/images",
            @"/webedit",
            @"/layouts",
            @"/feed"
        };
    }
}