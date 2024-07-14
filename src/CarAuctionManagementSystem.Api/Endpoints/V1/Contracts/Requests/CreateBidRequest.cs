namespace CarAuctionManagementSystem.Api.Endpoints.V1.Contracts.Requests;

public record CreateBidRequest(double Value, string UserIdentifier);