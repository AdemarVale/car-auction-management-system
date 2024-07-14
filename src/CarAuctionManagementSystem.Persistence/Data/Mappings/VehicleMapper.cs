using CarAuctionManagementSystem.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionManagementSystem.Persistence.Data.Mappings;

internal static class VehicleMapper
{
    public static void MapVehicles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>(
            builder =>
            {
                builder.HasKey(r => r.Id);
                builder.Property(r => r.Id).ValueGeneratedOnAdd();

                builder.Property(r => r.CreatedAt).HasDefaultValue(DateTimeOffset.UnixEpoch);
                builder.Property(r => r.UpdatedAt).HasDefaultValue(DateTimeOffset.UnixEpoch);

                builder
                    .HasDiscriminator<VehicleType>("type")
                    .HasValue<Sedan>(VehicleType.Sedan)
                    .HasValue<Hatchback>(VehicleType.Hatchback)
                    .HasValue<Suv>(VehicleType.Suv)
                    .HasValue<Truck>(VehicleType.Truck);

                builder
                    .HasIndex(x => x.Identifier)
                    .IsUnique();

                builder.Property(x => x.Manufacturer).HasMaxLength(512).IsRequired();
                builder.Property(x => x.Model).HasMaxLength(512).IsRequired();
                builder.Property(x => x.Year).IsRequired();
                builder.Property(x => x.StartingBid).IsRequired();

                builder.ToTable("vehicle");
            });
    }
}