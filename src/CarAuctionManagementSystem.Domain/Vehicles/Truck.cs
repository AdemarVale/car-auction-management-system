namespace CarAuctionManagementSystem.Domain.Vehicles;
public class Truck : Vehicle
{
    public double LoadCapacity { get; set; }

    public Truck(
        double loadCapacity,
        string identifier,
        string manufacturer,
        string model,
        int year,
        double startingBid) : base(identifier, manufacturer, model, year, startingBid)
    {
        LoadCapacity = loadCapacity;
    }
}
