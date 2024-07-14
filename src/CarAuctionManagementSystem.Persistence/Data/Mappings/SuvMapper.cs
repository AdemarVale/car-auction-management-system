using CarAuctionManagementSystem.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionManagementSystem.Persistence.Data.Mappings;

internal static class SuvMapper
{
    public static void MapSuvs(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Suv>(
            builder =>
            {
                builder.Property(x => x.NumberOfSeats).IsRequired();
            });
    }
}