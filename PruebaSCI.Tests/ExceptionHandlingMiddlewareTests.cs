using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using PruebaSCI.Api.Middleware;

namespace PruebaSCI.Tests;

public sealed class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenRequestFails_ReturnsSpanishProblemDetails()
    {
        var context = new DefaultHttpContext();
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        var middleware = new ExceptionHandlingMiddleware(
            _ => throw new InvalidOperationException("Sensitive internal detail"),
            NullLogger<ExceptionHandlingMiddleware>.Instance);

        await middleware.InvokeAsync(context);

        responseBody.Position = 0;
        using var document = await System.Text.Json.JsonDocument.ParseAsync(responseBody);
        var root = document.RootElement;

        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Equal("application/problem+json", context.Response.ContentType);
        Assert.Equal("Se produjo un error inesperado.", root.GetProperty("title").GetString());
        Assert.DoesNotContain("Sensitive internal detail", root.GetProperty("detail").GetString());
    }
}
