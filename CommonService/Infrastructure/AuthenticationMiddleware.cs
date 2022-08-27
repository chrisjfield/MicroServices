using Microsoft.Extensions.Logging;

namespace CommonService.Infrastructure;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEY = "X-ClientId";
    private readonly ILogger<AuthenticationMiddleware> _logger;

    public AuthenticationMiddleware(RequestDelegate next, ILoggerFactory _loggerFactory)
    {
        _next = next;
        _logger = _loggerFactory.CreateLogger<AuthenticationMiddleware>(); ;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(APIKEY, out var extractedApiKey))
        {
            _logger.LogInformation("No Api Key provided");
            throw new UnauthorizedAccessException("Client Id was not provided");
        }


        await _next(context);
    }
}
