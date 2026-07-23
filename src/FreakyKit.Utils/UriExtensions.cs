using System.Net;

namespace FreakyKit.Utils;

public static class UriExtensions
{
    /// <summary>
    /// Returns a new <see cref="Uri"/> with <paramref name="key"/>=<paramref name="value"/> appended to its query string.
    /// Existing parameters are preserved; both pieces are URL-encoded.
    /// </summary>
    /// <param name="uri">The base URI.</param>
    /// <param name="key">Query parameter name.</param>
    /// <param name="value">Query parameter value.</param>
    public static Uri AppendQueryParameter(this Uri uri, string key, string value)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);

        var builder = new UriBuilder(uri);
        var query = builder.Query.TrimStart('?');
        var addition = $"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(value)}";
        builder.Query = string.IsNullOrEmpty(query) ? addition : $"{query}&{addition}";
        return builder.Uri;
    }

    /// <summary>
    /// Parses the query string of <paramref name="uri"/> into a key/value dictionary.
    /// Repeated keys keep the last value. Keys and values are URL-decoded.
    /// </summary>
    /// <param name="uri">The URI to parse.</param>
    public static IReadOnlyDictionary<string, string> GetQueryParameters(this Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        var query = uri.Query.TrimStart('?');
        if (query.Length == 0) return result;

        foreach (var pair in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var eq = pair.IndexOf('=');
            if (eq < 0)
            {
                result[WebUtility.UrlDecode(pair)] = string.Empty;
            }
            else
            {
                var key = WebUtility.UrlDecode(pair[..eq]);
                var value = WebUtility.UrlDecode(pair[(eq + 1)..]);
                result[key] = value;
            }
        }
        return result;
    }

    /// <summary>
    /// Returns a new <see cref="Uri"/> identical to <paramref name="uri"/> but with the query string removed.
    /// Fragments are preserved.
    /// </summary>
    /// <param name="uri">The URI to strip.</param>
    public static Uri WithoutQuery(this Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        var builder = new UriBuilder(uri) { Query = string.Empty };
        return builder.Uri;
    }

    /// <summary>
    /// Returns a new <see cref="Uri"/> whose path ends with a slash. No-op when already trailing-slashed.
    /// </summary>
    /// <param name="uri">The URI to normalize.</param>
    public static Uri EnsureTrailingSlash(this Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        if (uri.AbsolutePath.EndsWith('/')) return uri;
        var builder = new UriBuilder(uri) { Path = uri.AbsolutePath + "/" };
        return builder.Uri;
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="uri"/>'s host is <c>localhost</c> (case-insensitive) or
    /// an IP loopback address (127.0.0.0/8 or ::1 / IPv6 loopback).
    /// </summary>
    /// <param name="uri">The URI to inspect.</param>
    public static bool IsLocalhost(this Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        var host = uri.Host;
        if (string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase))
            return true;

        if (IPAddress.TryParse(host, out var ip))
            return IPAddress.IsLoopback(ip);

        return false;
    }
}
