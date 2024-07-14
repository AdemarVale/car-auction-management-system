using CarAuctionManagementSystem.Infrastructure.Dependencies;

namespace CarAuctionManagementSystem.Api.Extensions;

public static class InfrastuctureExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddMediator();
    }
}
