namespace CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Dtos;
public record AuctionDto(DateTimeOffset CreatedAt, BidDto? BiggestBid);
