using CarAuctionManagementSystem.Application.Common.Abstractions;
using CarAuctionManagementSystem.Application.Common.Dtos;
using CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Dtos;
using MediatR;

namespace CarAuctionManagementSystem.Application.Features.Vehicles.Queries;

public class GetVehiclesQuery : IRequest<PagedResult<VehicleDto>>
{
    public int? PageNumber { get; }

    public int? PageSize { get; }

    public string? Type { get; }

    public string? Manufacturer { get; }

    public string? Model { get; }

    public int? Year { get; }

    public GetVehiclesQuery(int? pageNumber, int? pageSize, string? type, string? manufacturer, string? model, int? year)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Type = type;
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
    }
}
