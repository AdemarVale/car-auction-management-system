using CarAuctionManagementSystem.Domain.Abstractions;
using CarAuctionManagementSystem.Domain.Auctions;

namespace CarAuctionManagementSystem.Domain.Bids;
public class Bid : BaseEntity
{
    public double Value { get; }

    public string UserIdentifier { get; }

    // Foreign key to Auction entity
    public long AuctionId { get; }

    public virtual Auction Auction { get; }

    public Bid(double value, string userIdentifier)
    {
        Value = value;
        UserIdentifier = userIdentifier;
    }
}
