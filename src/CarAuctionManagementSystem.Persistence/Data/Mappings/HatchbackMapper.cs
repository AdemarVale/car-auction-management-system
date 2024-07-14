using CarAuctionManagementSystem.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionManagementSystem.Persistence.Data.Mappings;

internal static class HatchbackMapper
{
    public static void MapHatchbacks(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hatchback>(
            builder =>
            {
                builder.Property(x => x.NumberOfDoors).IsRequired();
            });
    }
}