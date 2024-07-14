namespace CarAuctionManagementSystem.Application.Common.Dtos;

public class PagedResult<T>
{
    public IEnumerable<T> Results { get; }

    public int TotalResultsCount { get; }

    public int TotalPages { get; }

    public PagedResult(IEnumerable<T> results, int totalResultsCount, int totalPages)
    {
        Results = results;
        TotalResultsCount = totalResultsCount;
        TotalPages = totalPages;
    }
}