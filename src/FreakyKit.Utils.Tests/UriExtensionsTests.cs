namespace FreakyKit.Utils.Tests;

public class UriExtensionsTests
{
    [Fact]
    public void AppendQueryParameter_NoExistingQuery()
    {
        var uri = new Uri("https://example.com/api");

        var result = uri.AppendQueryParameter("x", "1");

        Assert.Equal("x=1", result.Query.TrimStart('?'));
    }

    [Fact]
    public void AppendQueryParameter_AppendsToExisting()
    {
        var uri = new Uri("https://example.com/api?a=1");

        var result = uri.AppendQueryParameter("b", "2");

        Assert.Equal("a=1&b=2", result.Query.TrimStart('?'));
    }

    [Fact]
    public void AppendQueryParameter_UrlEncodesValue()
    {
        var uri = new Uri("https://example.com/");

        var result = uri.AppendQueryParameter("q", "a b&c");

        Assert.Contains("q=a+b%26c", result.Query);
    }

    [Fact]
    public void GetQueryParameters_ParsesAll()
    {
        var uri = new Uri("https://example.com/?a=1&b=two&c=");

        var qp = uri.GetQueryParameters();

        Assert.Equal("1", qp["a"]);
        Assert.Equal("two", qp["b"]);
        Assert.Equal(string.Empty, qp["c"]);
    }

    [Fact]
    public void GetQueryParameters_NoQuery_ReturnsEmpty()
    {
        var uri = new Uri("https://example.com/path");

        Assert.Empty(uri.GetQueryParameters());
    }

    [Fact]
    public void GetQueryParameters_UrlDecodes()
    {
        var uri = new Uri("https://example.com/?q=hello+world");

        Assert.Equal("hello world", uri.GetQueryParameters()["q"]);
    }

    [Fact]
    public void WithoutQuery_StripsQuery()
    {
        var uri = new Uri("https://example.com/path?a=1&b=2");

        var result = uri.WithoutQuery();

        Assert.Equal(string.Empty, result.Query);
        Assert.Equal("/path", result.AbsolutePath);
    }

    [Fact]
    public void WithoutQuery_PreservesFragment()
    {
        var uri = new Uri("https://example.com/path?a=1&b=2#section");

        var result = uri.WithoutQuery();

        Assert.Equal(string.Empty, result.Query);
        Assert.Equal("/path", result.AbsolutePath);
        Assert.Equal("#section", result.Fragment);
    }

    [Fact]
    public void EnsureTrailingSlash_AddsWhenMissing()
    {
        var uri = new Uri("https://example.com/api");

        var result = uri.EnsureTrailingSlash();

        Assert.EndsWith("/", result.AbsolutePath);
    }

    [Fact]
    public void EnsureTrailingSlash_AlreadyTrailing_NoOp()
    {
        var uri = new Uri("https://example.com/api/");

        Assert.Equal(uri.AbsoluteUri, uri.EnsureTrailingSlash().AbsoluteUri);
    }

    [Theory]
    [InlineData("http://localhost", true)]
    [InlineData("http://LOCALHOST:8080", true)]
    [InlineData("http://127.0.0.1", true)]
    [InlineData("http://127.0.0.2", true)]
    [InlineData("http://127.255.255.255", true)]
    [InlineData("http://[::1]", true)]
    [InlineData("http://example.com", false)]
    [InlineData("http://192.168.1.1", false)]
    [InlineData("http://127.1.0.0", true)]
    public void IsLocalhost(string urlString, bool expected)
    {
        var uri = new Uri(urlString);

        Assert.Equal(expected, uri.IsLocalhost());
    }
}
