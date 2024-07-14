using CarAuctionManagementSystem.Application.Common.Abstractions;
using CarAuctionManagementSystem.Application.Common.Dtos;
using CarAuctionManagementSystem.Application.Features.Vehicles.Queries;
using CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Handlers;
using CarAuctionManagementSystem.Domain.Vehicles;
using CarAuctionManagementSystem.Utilities.Builders;
using FluentAssertions;
using Moq;
using Xunit;

namespace CarAuctionManagementSystem.UnitTests.Tests.Application.Features.Vehicles.Queries.Handlers;

public class GetVehicleQueryHandlerTests
{
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
    private readonly GetVehiclesQueryHandler _handler;

    public GetVehicleQueryHandlerTests()
    {
        _vehicleRepositoryMock = new Mock<IVehicleRepository>();
        _handler = new GetVehiclesQueryHandler(_vehicleRepositoryMock.Object);
    }

    [Fact]
    public async Task GetVehicles_NullQueryParams_ShouldReturnMappedVehicles()
    {
        // Arrange
        var vehicle = new VehicleBuilder().WithBid(10000).Build();
        var pagedResult = new PagedResult<Vehicle>([vehicle], 1, 1);

        var request = new GetVehiclesQuery(null, null, null, null, null, null);

        _vehicleRepositoryMock
            .Setup(x => x.GetVehiclesWithPaginationAsync(1, 15, null, null, null, null, CancellationToken.None))
            .ReturnsAsync(pagedResult)
            .Verifiable();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _vehicleRepositoryMock.Verify();

        result.TotalResultsCount.Should().Be(pagedResult.TotalResultsCount);
        result.TotalPages.Should().Be(pagedResult.TotalPages);
        result.Results.Count().Should().Be(pagedResult.Results.Count());
    }

    [Fact]
    public async Task GetVehicles_WithQueryParams_ShouldReturnMappedVehicles()
    {
        // Arrange
        var vehicle = new VehicleBuilder().WithBid(10000).Build();
        var pagedResult = new PagedResult<Vehicle>([vehicle], 1, 1);

        var request = new GetVehiclesQuery(2, 20, "sedan", "manufacturer", "model", 2024);

        _vehicleRepositoryMock
            .Setup(x => x.GetVehiclesWithPaginationAsync(
                request.PageNumber!.Value,
                request.PageSize!.Value,
                Enum.Parse<VehicleType>(request.Type!, true),
                request.Manufacturer,
                request.Model,
                request.Year,
                CancellationToken.None))
            .ReturnsAsync(pagedResult)
            .Verifiable();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _vehicleRepositoryMock.Verify();

        result.TotalResultsCount.Should().Be(pagedResult.TotalResultsCount);
        result.TotalPages.Should().Be(pagedResult.TotalPages);
        result.Results.Count().Should().Be(pagedResult.Results.Count());
    }
}
