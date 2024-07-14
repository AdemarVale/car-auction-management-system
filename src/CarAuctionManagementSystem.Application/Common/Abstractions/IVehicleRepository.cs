using CarAuctionManagementSystem.Application.Common.Dtos;
using CarAuctionManagementSystem.Domain.Vehicles;

namespace CarAuctionManagementSystem.Application.Common.Abstractions;

public interface IVehicleRepository
{
    Task<PagedResult<Vehicle>> GetVehiclesWithPaginationAsync(
        int pageNumber,
        int pageSize,
        VehicleType? type,
        string? manufacturer,
        string? model,
        int? year,
        CancellationToken cancellationToken);

    Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken);

    Task<bool> IsUniqueAsync(string identifier, CancellationToken cancellationToken);

    Task<Vehicle?> GetVehicleWithOpenAuctionsAsync(string identifier, CancellationToken cancellationToken);

    Task<Vehicle?> GetVehicleWithBiggestBidAsync(string identifier, CancellationToken cancellationToken);
}
