using Sitecore.Diagnostics;
using Sitecore.Foundation.ItemResolver.Providers;
using System;

namespace Sitecore.Foundation.ItemResolver.Events.PublishEnd
{
    /// <summary>
    /// Wildcard Cache Purge Processor
    /// </summary>
    public class WildcardsCachePurgeProcessor
    {
        /// <summary>
        /// Clear cache
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Args</param>
        public void ClearCache(object sender, EventArgs args)
        {
            try
            {
                // Clear cache
                WildcardManager.Current?.ClearCache();
                WildcardItemResolver.Current?.ClearCache();
            }
            catch (Exception ex)
            {
                Log.Error($"An error occured on WildcardsCachePurgeProcessor: {ex.InnerException}", this);
            }
        }
    }
}