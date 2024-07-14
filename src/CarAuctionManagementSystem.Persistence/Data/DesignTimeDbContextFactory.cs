using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarAuctionManagementSystem.Persistence.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CarAuctionManagementSystemDbContext>
{
    public CarAuctionManagementSystemDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CarAuctionManagementSystemDbContext>();
        optionsBuilder.UseNpgsql(args[0]);

        return new CarAuctionManagementSystemDbContext(optionsBuilder.Options);
    }
}
