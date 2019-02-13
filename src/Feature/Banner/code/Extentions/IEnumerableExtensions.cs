using System;
using System.Collections.Generic;

namespace Sitecore.Feature.Banner.Extentions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Map<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }
    }
}