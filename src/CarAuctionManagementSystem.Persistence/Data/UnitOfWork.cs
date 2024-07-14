using CarAuctionManagementSystem.Application.Common.Abstractions;
using CarAuctionManagementSystem.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionManagementSystem.Persistence.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly CarAuctionManagementSystemDbContext _dbContext;

    public UnitOfWork(CarAuctionManagementSystemDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        UpdateAuditData();

        // After this line runs, all the changes (from the Command Handler and Domain
        // event handlers) performed through the DbContext will be committed
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditData()
    {
        foreach (var entry in _dbContext.ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity entity)
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTimeOffset.UtcNow;
                    entity.UpdatedAt = entity.CreatedAt;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTimeOffset.UtcNow;
                }
            }
        }
    }
}
