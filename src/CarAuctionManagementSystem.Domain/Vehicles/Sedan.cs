namespace CarAuctionManagementSystem.Domain.Vehicles;
public class Sedan : Vehicle
{
    public int NumberOfDoors { get; }

    public Sedan(
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
