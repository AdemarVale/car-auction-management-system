namespace CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Dtos;

public record VehicleDto(
    string Type,
    string Identifier,
    string Manufacturer,
    string Model,
    int Year,
    double StartingBid,
    int? NumberOfSeats,
    int? NumberOfDoors,
    double? LoadCapacity,
    AuctionDto? ActiveAuction);
