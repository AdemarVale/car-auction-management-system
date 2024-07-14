using CarAuctionManagementSystem.Application.Common.Abstractions;
using CarAuctionManagementSystem.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionManagementSystem.Api.Extensions;

public static class PersistenceExtensions
{
    public static void AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CarAuctionManagementSystemDbContext>(o =>
            o.UseNpgsql(
                configuration.GetConnectionString("CarAuctionManagementSystem"),
                options => options.EnableRetryOnFailure()));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IVehicleRepository, VehicleRepository>();
    }

    public static void ApplyMigrations(this IApplicationBuilder app, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("EnableAutomaticMigrations"))
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CarAuctionManagementSystemDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
