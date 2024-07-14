namespace CarAuctionManagementSystem.Domain.Vehicles;
public class Suv : Vehicle
{
    public int NumberOfSeats { get; }

    public Suv(
        int numberOfSeats,
        string identifier,
        string manufacturer,
        string model,
        int year,
        double startingBid) : base(identifier, manufacturer, model, year, startingBid)
    {
        NumberOfSeats = numberOfSeats;
    }
}
