using CarAuctionManagementSystem.Application.Common.Abstractions;
using CarAuctionManagementSystem.Application.Common.Dtos;
using CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Dtos;
using CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Mappers;
using CarAuctionManagementSystem.Domain.Vehicles;
using MediatR;

namespace CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Handlers;
public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, PagedResult<VehicleDto>>
{
    private readonly IVehicleRepository _vehicleRepository;

    public GetVehiclesQueryHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<PagedResult<VehicleDto>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehiclesPaginatedResult = await _vehicleRepository.GetVehiclesWithPaginationAsync(
            pageNumber: request.PageNumber ?? 1,
            pageSize: request.PageSize ?? 15,
            type: Enum.TryParse<VehicleType>(request.Type, true, out var type) ? type : null,
            manufacturer: request.Manufacturer,
            model: request.Model,
            year: request.Year,
            cancellationToken: cancellationToken);

        return new PagedResult<VehicleDto>(
            results: vehiclesPaginatedResult.Results.Select(x => x.MapToDto()),
            totalResultsCount: vehiclesPaginatedResult.TotalResultsCount,
            totalPages: vehiclesPaginatedResult.TotalPages);
    }
}
