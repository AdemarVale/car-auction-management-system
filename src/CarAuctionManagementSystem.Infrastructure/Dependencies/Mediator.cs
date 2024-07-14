using CarAuctionManagementSystem.Application.Features.Vehicles.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace CarAuctionManagementSystem.Infrastructure.Dependencies;

public static class Mediator
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        var applicationAssembly = typeof(CreateVehicleCommand).Assembly;

        return services
            .AddMediatR(c =>
            {
                c.Lifetime = ServiceLifetime.Scoped;
                c.RegisterServicesFromAssembly(applicationAssembly);
            });
    }
}
