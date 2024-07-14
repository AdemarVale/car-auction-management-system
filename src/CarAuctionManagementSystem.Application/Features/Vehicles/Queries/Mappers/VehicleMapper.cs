using System.ComponentModel;
using CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Dtos;
using CarAuctionManagementSystem.Domain.Vehicles;

namespace CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Mappers;
public static class VehicleMapper
{
    public static VehicleDto MapToDto(this Vehicle vehicle)
    {
        return vehicle switch
        {
            Sedan sedanvehicle => new VehicleDto(
                Type: nameof(Sedan),
                Identifier: sedanvehicle.Identifier,
                Manufacturer: sedanvehicle.Manufacturer,
                Model: sedanvehicle.Model,
                Year: sedanvehicle.Year,
                StartingBid: sedanvehicle.StartingBid,
                NumberOfSeats: null,
                NumberOfDoors: sedanvehicle.NumberOfDoors,
                LoadCapacity: null,
                ActiveAuction: BuildAuction(sedanvehicle)),

            Suv suvvehicle => new VehicleDto(
                Type: nameof(Suv),
                Identifier: suvvehicle.Identifier,
                Manufacturer: suvvehicle.Manufacturer,
                Model: suvvehicle.Model,
                Year: suvvehicle.Year,
                StartingBid: suvvehicle.StartingBid,
                NumberOfSeats: suvvehicle.NumberOfSeats,
                NumberOfDoors: null,
                LoadCapacity: null,
                ActiveAuction: BuildAuction(suvvehicle)),

            Hatchback hatchBackvehicle => new VehicleDto(
                Type: nameof(Hatchback),
                Identifier: hatchBackvehicle.Identifier,
                Manufacturer: hatchBackvehicle.Manufacturer,
                Model: hatchBackvehicle.Model,
                Year: hatchBackvehicle.Year,
                StartingBid: hatchBackvehicle.StartingBid,
                NumberOfSeats: null,
                NumberOfDoors: hatchBackvehicle.NumberOfDoors,
                LoadCapacity: null,
                ActiveAuction: BuildAuction(hatchBackvehicle)),

            Truck truckvehicle => new VehicleDto(
                Type: nameof(Truck),
                Identifier: truckvehicle.Identifier,
                Manufacturer: truckvehicle.Manufacturer,
                Model: truckvehicle.Model,
                Year: truckvehicle.Year,
                StartingBid: truckvehicle.StartingBid,
                NumberOfSeats: null,
                NumberOfDoors: null,
                LoadCapacity: truckvehicle.LoadCapacity,
                ActiveAuction: BuildAuction(truckvehicle)),

            _ => throw new InvalidEnumArgumentException(),
        };;
    }

    private static AuctionDto? BuildAuction(Vehicle vehicle)
    {
        if (vehicle.Auctions is null || vehicle.Auctions.Count == 0)
        {
            return null;
        }

        var auction = vehicle.Auctions.Single();

        if (auction.Bids is null || auction.Bids.Count == 0)
        {
            return new AuctionDto(auction.CreatedAt, null);
        }

        var biggestBid = auction.Bids.Single();

        return new AuctionDto(auction.CreatedAt, new BidDto(biggestBid.CreatedAt, biggestBid.Value, biggestBid.UserIdentifier));
    }
}
