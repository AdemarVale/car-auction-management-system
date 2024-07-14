using FluentResults;
using MediatR;

namespace CarAuctionManagementSystem.Application.Features.Vehicles.Commands;
public class CreateVehicleCommand : IRequest<Result>
{
    public string Type { get; }

    public string Identifier { get; }

    public string Manufacturer { get; }

    public string Model { get; }

    public int Year { get; }

    public double StartingBid { get; }

    public int? NumberOfDoors { get; }

    public int? NumberOfSeats { get; }

    public double? LoadCapacity { get; }

    public CreateVehicleCommand(
        string type,
        string identifier,
        string manufacturer,
        string model,
        int year,
        double startingBid,
        int? numberOfDoors,
        int? numberOfSeats,
        double? loadCapacity)
    {
        Type = type;
        Identifier = identifier;
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        StartingBid = startingBid;
        NumberOfDoors = numberOfDoors;
        NumberOfSeats = numberOfSeats;
        LoadCapacity = loadCapacity;
    }
}
