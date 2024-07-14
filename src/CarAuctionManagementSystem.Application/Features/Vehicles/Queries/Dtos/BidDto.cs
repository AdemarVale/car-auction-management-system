namespace CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Dtos;

public record BidDto(DateTimeOffset CreatedAt, double Value, string UserIdentifier);
