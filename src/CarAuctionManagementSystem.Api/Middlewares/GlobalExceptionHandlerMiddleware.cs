using System.Net;
using System.Text.Json;
using CarAuctionManagementSystem.Api.Endpoints.V1.Contracts.Responses;

namespace CarAuctionManagementSystem.Api.Middlewares;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception caugh by middleware: {Message}.", ex.Message);

            var errorResponse = new ErrorResponse(
                status: 500,
                errors: new Dictionary<string, List<string>>() { { "Exception", new List<string>() { ex.Message } } });

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
