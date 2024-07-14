using CarAuctionManagementSystem.Domain.Bids;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionManagementSystem.Persistence.Data.Mappings;

internal static class BidMapper
{
    public static void MapBids(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bid>(
            builder =>
            {
                builder.HasKey(r => r.Id);
                builder.Property(r => r.Id).ValueGeneratedOnAdd();

                builder.Property(r => r.CreatedAt).HasDefaultValue(DateTimeOffset.UnixEpoch);
                builder.Property(r => r.UpdatedAt).HasDefaultValue(DateTimeOffset.UnixEpoch);

                builder.Property(r => r.Value).IsRequired();
                builder.Property(r => r.UserIdentifier).IsRequired();

                builder
                    .HasOne(r => r.Auction)
                    .WithMany(r => r.Bids)
                    .HasForeignKey(r => r.AuctionId);

                builder.ToTable("auction_bid");
            });
    }
}