using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Selenium.AntiCaptcha.Extensions;

/// <summary>
/// Manages caching of site keys to improve performance for repeated visits to the same sites
/// </summary>
public static class SiteKeyCacheManager
{
    private static readonly ConcurrentDictionary<string, string> SiteKeyCache = new();
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromHours(24);
    private static readonly Dictionary<string, DateTime> CacheExpirations = new();

    /// <summary>
    /// Gets a cached site key for the given URL if available
    /// </summary>
    /// <param name="url">Website URL</param>
    /// <returns>Cached site key or null if not available</returns>
    public static string? GetCachedSiteKey(string url)
    {
        if (SiteKeyCache.TryGetValue(url, out var siteKey))
        {
            if (CacheExpirations.TryGetValue(url, out var expiration) && expiration > DateTime.UtcNow)
            {
                return siteKey;
            }
            
            // Expired
            SiteKeyCache.TryRemove(url, out _);
            CacheExpirations.Remove(url);
        }
        
        return null;
    }

    /// <summary>
    /// Caches a site key for the given URL
    /// </summary>
    /// <param name="url">Website URL</param>
    /// <param name="siteKey">Site key to cache</param>
    public static void CacheSiteKey(string url, string siteKey)
    {
        if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(siteKey))
            return;
            
        SiteKeyCache[url] = siteKey;
        CacheExpirations[url] = DateTime.UtcNow.Add(CacheExpiration);
    }
}
