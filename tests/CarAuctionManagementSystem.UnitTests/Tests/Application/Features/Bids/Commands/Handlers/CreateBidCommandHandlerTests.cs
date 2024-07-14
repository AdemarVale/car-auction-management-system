using CarAuctionManagementSystem.Application.Common.Abstractions;
using CarAuctionManagementSystem.Application.Features.Bids.Commands;
using CarAuctionManagementSystem.Application.Features.Bids.Commands.Handlers;
using CarAuctionManagementSystem.Domain.Vehicles;
using CarAuctionManagementSystem.Utilities.Builders;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace CarAuctionManagementSystem.UnitTests.Tests.Application.Features.Auctions.Commands.Handlers;

public class CreateBidCommandHandlerTests
{
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CreateBidCommandHandler>> _loggerMock;

    private readonly CreateBidCommandHandler _handler;

    public CreateBidCommandHandlerTests()
    {
        _vehicleRepositoryMock = new Mock<IVehicleRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<CreateBidCommandHandler>>();

        _handler = new CreateBidCommandHandler(_vehicleRepositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ValidationErrror_ReturnsError()
    {
        // Arrange
        var request = new CreateBidCommand("identifier", 0, "userIdentifier");

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(1);
        result.Errors[0].Message.Should().Be("Invalid value.");
    }

    [Fact]
    public async Task Handle_VehicleNotFound_ReturnsError()
    {
        // Arrange
        var request = new CreateBidCommand("identifier", 10000, "userIdentifier");

        _vehicleRepositoryMock
            .Setup(x => x.GetVehicleWithBiggestBidAsync(request.VehicleIdentifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Vehicle)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _vehicleRepositoryMock.Verify();
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(1);
        result.Errors[0].Message.Should().Be("Vehicle not found.");
    }

    [Fact]
    public async Task Handle_NoOpenAuctions_ReturnsError()
    {
        // Arrange
        var request = new CreateBidCommand("identifier", 10000, "userIdentifier");

        var vehicle = new VehicleBuilder().Build();

        _vehicleRepositoryMock
            .Setup(repo => repo.GetVehicleWithBiggestBidAsync(request.VehicleIdentifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle)
            .Verifiable();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _vehicleRepositoryMock.Verify();
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(1);
        result.Errors[0].Message.Should().Be("There are no open auctions.");
    }

    [Fact]
    public async Task Handle_BidValueNotBiggerThanPrevious_ReturnsError()
    {
        // Arrange
        var request = new CreateBidCommand("identifier", 10000, "userIdentifier");

        var vehicle = new VehicleBuilder().WithBid(11000).Build();

        _vehicleRepositoryMock
            .Setup(repo => repo.GetVehicleWithBiggestBidAsync(request.VehicleIdentifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _vehicleRepositoryMock.Verify();
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(1);
        result.Errors[0].Message.Should().Be("There is already a bigger or equal bid for this vehicle.");
    }

    [Fact]
    public async Task Handle_BidValueNotBiggerThanStartingBid_ReturnsError()
    {
        // Arrange
        var vehicle = new VehicleBuilder().WithOpenAuction().Build();

        var request = new CreateBidCommand("identifier", vehicle.StartingBid - 1, "userIdentifier");

        _vehicleRepositoryMock
            .Setup(repo => repo.GetVehicleWithBiggestBidAsync(request.VehicleIdentifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _vehicleRepositoryMock.Verify();
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(1);
        result.Errors[0].Message.Should().Be("There is already a bigger or equal bid for this vehicle.");
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesBid()
    {
        // Arrange
        var request = new CreateBidCommand("identifier", 5500, "userIdentifier");

        var vehicle = new VehicleBuilder().WithBid(5200).Build();

        _vehicleRepositoryMock
            .Setup(repo => repo.GetVehicleWithBiggestBidAsync(request.VehicleIdentifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _vehicleRepositoryMock.Verify();
        result.IsSuccess.Should().BeTrue();
        vehicle.Auctions.Single().Bids.MaxBy(x => x.Value)!.Value.Should().Be(5500);
        vehicle.Auctions.Single().Bids.MaxBy(x => x.Value)!.UserIdentifier.Should().Be("userIdentifier");
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
    }
}
