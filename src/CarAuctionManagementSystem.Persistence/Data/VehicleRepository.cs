using CarAuctionManagementSystem.Application.Common.Abstractions;
using CarAuctionManagementSystem.Application.Common.Dtos;
using CarAuctionManagementSystem.Domain.Auctions;
using CarAuctionManagementSystem.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionManagementSystem.Persistence.Data;

public class VehicleRepository : IVehicleRepository
{
    private readonly DbSet<Vehicle> _dbSet;

    public VehicleRepository(CarAuctionManagementSystemDbContext dbContext)
    {
        _dbSet = dbContext.Set<Vehicle>();
    }

    public async Task<PagedResult<Vehicle>> GetVehiclesWithPaginationAsync(
        int pageNumber,
        int pageSize,
        VehicleType? type,
        string? manufacturer,
        string? model,
        int? year,
        CancellationToken cancellationToken)
    {
        var query = _dbSet.AsNoTracking();

        if (type is not null)
        {
            query = BuildQueryByType(query, type);
        }

        if (manufacturer is not null)
        {
            query = query.Where(x => x.Manufacturer == manufacturer);
        }

        if (model is not null)
        {
            query = query.Where(x => x.Model == model);
        }

        if (year is not null)
        {
            query = query.Where(x => x.Year == year);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = query
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize);

        query = query
            .Include(x => x.Auctions.Where(x => x.Status == AuctionStatus.Open))
            .ThenInclude(x => x.Bids.OrderByDescending(x => x.Value).Take(1));

        var results = await query.ToListAsync(cancellationToken);

        var totalPages = (decimal)totalCount / pageSize;

        return new PagedResult<Vehicle>(results, totalCount, (int)Math.Ceiling(totalPages));
    }

    public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(vehicle, cancellationToken);
    }

    public async Task<bool> IsUniqueAsync(string identifier, CancellationToken cancellationToken)
    {
        return !await _dbSet.AnyAsync(x => x.Identifier == identifier, cancellationToken);
    }

    public async Task<Vehicle?> GetVehicleWithOpenAuctionsAsync(string identifier, CancellationToken cancellationToken)
    {
        return await _dbSet
                    .Include(x => x.Auctions.Where(x => x.Status == AuctionStatus.Open))
                    .SingleOrDefaultAsync(x => x.Identifier == identifier, cancellationToken);
    }

    public async Task<Vehicle?> GetVehicleWithBiggestBidAsync(string identifier, CancellationToken cancellationToken)
    {
        return await _dbSet
                    .Include(x => x.Auctions.Where(x => x.Status == AuctionStatus.Open))
                    .ThenInclude(x => x.Bids.OrderByDescending(x => x.Value).Take(1))
                    .SingleOrDefaultAsync(x => x.Identifier == identifier, cancellationToken);
    }

    private static IQueryable<Vehicle> BuildQueryByType(IQueryable<Vehicle> query, VehicleType? type)
    {
        return type switch
        {
            VehicleType.Sedan => query.OfType<Sedan>(),
            VehicleType.Truck => query.OfType<Truck>(),
            VehicleType.Hatchback => query.OfType<Hatchback>(),
            VehicleType.Suv => query.OfType<Suv>(),
            _ => query,
        };
    }
}
