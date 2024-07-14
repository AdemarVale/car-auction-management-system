using CarAuctionManagementSystem.Domain.Abstractions;
using CarAuctionManagementSystem.Domain.Auctions;

namespace CarAuctionManagementSystem.Domain.Vehicles;

public abstract class Vehicle : BaseEntity
{
    public string Identifier { get; }

    public string Manufacturer { get; }

    public string Model { get; }

    public int Year { get; }

    public double StartingBid { get; }

    // Navigation property
    public virtual ICollection<Auction> Auctions { get; set; }

    protected Vehicle(string identifier, string manufacturer, string model, int year, double startingBid)
    {
        Identifier = identifier;
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        StartingBid = startingBid;
    }

    public void StartAuction()
    {
        Auctions.Add(new Auction());
    }
}
