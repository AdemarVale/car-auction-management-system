using CarAuctionManagementSystem.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionManagementSystem.Persistence.Data.Mappings;

internal static class TruckMapper
{
    public static void MapTrucks(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Truck>(
            builder =>
            {
                builder.Property(x => x.LoadCapacity).IsRequired();
            });
    }
}