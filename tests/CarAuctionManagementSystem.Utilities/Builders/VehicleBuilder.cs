using CarAuctionManagementSystem.Domain.Auctions;
using CarAuctionManagementSystem.Domain.Bids;
using CarAuctionManagementSystem.Domain.Vehicles;

namespace CarAuctionManagementSystem.Utilities.Builders;
public class VehicleBuilder
{
    private readonly List<Auction> _auctions = [];

    public VehicleBuilder WithOpenAuction()
    {
        if (_auctions.Count == 0)
        {
            _auctions.Add(new Auction());
        }

        return this;
    }

    public VehicleBuilder WithBid(double value)
    {
        if (_auctions.Count == 0)
        {
            _auctions.Add(new Auction());
        }

        _auctions.Single().Bids = [new Bid(value, "identifier")];
        return this;
    }

    public Sedan Build()
    {
        return new Sedan(1, Guid.NewGuid().ToString(), "manufacturer", "model", 2024, 5000) { Auctions = _auctions };
    }
}
