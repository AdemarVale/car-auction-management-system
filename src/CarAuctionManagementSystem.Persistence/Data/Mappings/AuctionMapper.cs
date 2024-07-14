using CarAuctionManagementSystem.Domain.Auctions;
using CarAuctionManagementSystem.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionManagementSystem.Persistence.Data.Mappings;

internal static class AuctionMapper
{
    public static void MapAuctions(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auction>(
            builder =>
            {
                builder.HasKey(r => r.Id);
                builder.Property(r => r.Id).ValueGeneratedOnAdd();

                builder.Property(r => r.CreatedAt).HasDefaultValue(DateTimeOffset.UnixEpoch);
                builder.Property(r => r.UpdatedAt).HasDefaultValue(DateTimeOffset.UnixEpoch);

                builder.Property(x => x.Status).IsRequired();

                builder
                    .HasOne(x => x.Vehicle)
                    .WithMany(x => x.Auctions)
                    .HasForeignKey(x => x.VehicleId);

                builder
                    .HasMany(x => x.Bids)
                    .WithOne(x => x.Auction)
                    .HasForeignKey(x => x.AuctionId);

                builder.ToTable("vehicle_auction");
            });
    }
}