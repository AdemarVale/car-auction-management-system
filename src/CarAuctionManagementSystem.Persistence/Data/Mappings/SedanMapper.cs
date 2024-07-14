using CarAuctionManagementSystem.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionManagementSystem.Persistence.Data.Mappings;

internal static class SedanMapper
{
    public static void MapSedans(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sedan>(
            builder =>
            {
                builder.Property(x => x.NumberOfDoors).IsRequired();
            });
    }
}