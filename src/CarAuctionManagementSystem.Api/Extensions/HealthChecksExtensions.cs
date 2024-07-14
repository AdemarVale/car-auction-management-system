using System.Diagnostics.CodeAnalysis;
using CarAuctionManagementSystem.Persistence.Data;

namespace CarAuctionManagementSystem.Api.Extensions;

public static class HealthChecksExtensions
{
    [ExcludeFromCodeCoverage]
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddDbContextCheck<CarAuctionManagementSystemDbContext>(name: "db");

        return services;
    }
}
