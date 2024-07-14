using Microsoft.OpenApi.Models;

namespace CarAuctionManagementSystem.Api.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Car Auction Management System API v1",
                Description = "Car Auction Management System API v1",
            });
        });
    }
}
