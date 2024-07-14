using CarAuctionManagementSystem.Persistence.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionManagementSystem.Persistence.Data;

public class CarAuctionManagementSystemDbContext : DbContext
{
    public CarAuctionManagementSystemDbContext(DbContextOptions<CarAuctionManagementSystemDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.MapVehicles();
        modelBuilder.MapSedans();
        modelBuilder.MapHatchbacks();
        modelBuilder.MapSuvs();
        modelBuilder.MapTrucks();
        modelBuilder.MapAuctions();
        modelBuilder.MapBids();
    }
}
