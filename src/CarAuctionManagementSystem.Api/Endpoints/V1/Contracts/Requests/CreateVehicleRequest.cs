namespace CarAuctionManagementSystem.Api.Endpoints.V1.Contracts.Requests;

public record CreateVehicleRequest(
    string Type,
    string Identifier,
    string Manufacturer,
    string Model,
    int Year,
    double StartingBid,
    int? NumberOfDoors,
    int? NumberOfSeats,
    double? LoadCapacity);
