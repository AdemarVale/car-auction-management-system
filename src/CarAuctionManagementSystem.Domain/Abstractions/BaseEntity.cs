namespace CarAuctionManagementSystem.Domain.Abstractions;

public class BaseEntity
{
    public long Id { get; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
