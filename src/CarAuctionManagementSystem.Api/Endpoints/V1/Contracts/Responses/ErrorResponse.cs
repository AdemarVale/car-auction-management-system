namespace CarAuctionManagementSystem.Api.Endpoints.V1.Contracts.Responses;

public class ErrorResponse
{
    public string Title { get; } = "One or more errors occurred.";

    public int Status { get; }

    public Dictionary<string, List<string>> Errors { get; }

    public ErrorResponse(int status, Dictionary<string, List<string>> errors)
    {
        Status = status;
        Errors = errors;
    }
}