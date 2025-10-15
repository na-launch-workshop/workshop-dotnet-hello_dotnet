using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace workshop_dotnet_hello_dotnet.Tests;

public class HelloEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HelloEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetRoot_ReturnsEnglishTranslationWithTimestamp()
    {
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/");

        response.EnsureSuccessStatusCode();
        Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("hello world", content, StringComparison.OrdinalIgnoreCase);
        Assert.Matches(new Regex("@\\s*\\d{4}-\\d{2}-\\d{2}T"), content);
    }
}
