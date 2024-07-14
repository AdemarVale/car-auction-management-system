using CarAuctionManagementSystem.Domain.Auctions;
using CarAuctionManagementSystem.Domain.Bids;
using CarAuctionManagementSystem.Domain.Vehicles;
using CarAuctionManagementSystem.Persistence.Data;
using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace CarAuctionManagementSystem.IntegrationTests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private PostgreSqlContainer? container;

    public string ConnectionString
    {
        get
        {
            ArgumentNullException.ThrowIfNull(container);
            return container.GetConnectionString();
        }
    }

    public async Task InitializeAsync()
    {
        var testcontainersBuilder = new PostgreSqlBuilder()
            .WithPortBinding(5432, 5432)
            .WithExposedPort(5432)
            .WithDatabase("car-auction-management-system-tests")
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilCommandIsCompleted("pg_isready -t 5 -d car-auction-management-system-tests"));

        container = testcontainersBuilder.Build();
        await container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (container is not null)
        {
            await container.DisposeAsync();
        }
    }

    public CarAuctionManagementSystemDbContext CreateDbContext()
    {
        ArgumentNullException.ThrowIfNull(container);

        return new CarAuctionManagementSystemDbContext(
            new DbContextOptionsBuilder<CarAuctionManagementSystemDbContext>()
                   .UseNpgsql(container.GetConnectionString())
                   .Options);
    }

    public async Task<int> DeleteDataAsync()
    {
        using var dbContext = CreateDbContext();

        await dbContext.Set<Bid>().ExecuteDeleteAsync();
        await dbContext.Set<Auction>().ExecuteDeleteAsync();
        await dbContext.Set<Vehicle>().ExecuteDeleteAsync();

        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> CreateNewVehicle(Vehicle vehicle)
    {
        using var dbContext = CreateDbContext();

        dbContext.Set<Vehicle>().Add(vehicle);
        return await dbContext.SaveChangesAsync();
    }

    public Vehicle? GetVehicleByIdentifier(string identifier)
    {
        using var dbContext = CreateDbContext();

        return dbContext
            .Set<Vehicle>()
            .Include(x => x.Auctions)
            .ThenInclude(x => x.Bids)
            .SingleOrDefault(x => x.Identifier == identifier);
    }
}
