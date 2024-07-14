using CarAuctionManagementSystem.Domain.Abstractions;
using CarAuctionManagementSystem.Domain.Bids;
using CarAuctionManagementSystem.Domain.Vehicles;

namespace CarAuctionManagementSystem.Domain.Auctions;
public class Auction : BaseEntity
{
    public AuctionStatus Status { get; private set; }

    // Foreign key to vehicle entity
    public long VehicleId { get; }

    public virtual Vehicle Vehicle { get; set; }

    // Navigation property
    public virtual ICollection<Bid> Bids { get; set; }

    public Auction()
    {
        Status = AuctionStatus.Open;
    }

    public void Close()
    {
        Status = AuctionStatus.Closed;
    }
}
