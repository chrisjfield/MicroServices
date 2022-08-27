using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace UnitTests;

internal static class TestHelper
{
    internal static async Task<(HttpResponse, T?)> DeconstructIResult<T>(IResult result)
    {
        HttpContext mockHttpContext = CreateMockHttpContext();

        await result.ExecuteAsync(mockHttpContext);

        // Reset MemoryStream to start so we can read the response.
        mockHttpContext.Response.Body.Position = 0;

        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        T? responseBody = default;
        try
        {
            responseBody = await JsonSerializer.DeserializeAsync<T>(mockHttpContext.Response.Body, jsonOptions);
        }
        catch { }

        return (mockHttpContext.Response, responseBody);
    }

    private static HttpContext CreateMockHttpContext() => new DefaultHttpContext
    {
        // RequestServices needs to be set so the IResult implementation can log.
        RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider(),
        Response =
        {
            // The default response body is Stream.Null which throws away anything that is written to it.
            Body = new MemoryStream(),
        },
    };       
}
