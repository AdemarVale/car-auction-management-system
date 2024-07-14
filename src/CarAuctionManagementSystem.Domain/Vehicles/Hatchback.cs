namespace CarAuctionManagementSystem.Domain.Vehicles;
public class Hatchback : Vehicle
{
    public int NumberOfDoors { get; }

    public Hatchback(
        int numberOfDoors,
        string identifier,
        string manufacturer,
        string model,
        int year,
        double startingBid) : base(identifier, manufacturer, model, year, startingBid)
    {
        NumberOfDoors = numberOfDoors;
    }
}
