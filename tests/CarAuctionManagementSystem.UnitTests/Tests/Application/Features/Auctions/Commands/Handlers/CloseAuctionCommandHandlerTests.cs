using CarAuctionManagementSystem.Application.Common.Abstractions;
using CarAuctionManagementSystem.Application.Features.Auctions.Commands;
using CarAuctionManagementSystem.Application.Features.Auctions.Commands.Handlers;
using CarAuctionManagementSystem.Domain.Auctions;
using CarAuctionManagementSystem.Domain.Vehicles;
using CarAuctionManagementSystem.Utilities.Builders;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CarAuctionManagementSystem.UnitTests.Tests.Application.Features.Auctions.Commands.Handlers;

public class CloseAuctionCommandHandlerTests
{
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CloseAuctionCommandHandler>> _loggerMock;

    private readonly CloseAuctionCommandHandler _handler;

    public CloseAuctionCommandHandlerTests()
    {
        _vehicleRepositoryMock = new Mock<IVehicleRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<CloseAuctionCommandHandler>>();

        _handler = new CloseAuctionCommandHandler(_vehicleRepositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_VehicleNotFound_ReturnsError()
    {
        // Arrange
        var request = new CloseAuctionCommand("identifier");

        _vehicleRepositoryMock
            .Setup(x => x.GetVehicleWithOpenAuctionsAsync(request.VehicleIdentifier, It.IsAny<CancellationToken>()))
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
        var request = new CloseAuctionCommand("identifier");

        var vehicle = new VehicleBuilder().Build();

        _vehicleRepositoryMock
            .Setup(repo => repo.GetVehicleWithOpenAuctionsAsync(request.VehicleIdentifier, It.IsAny<CancellationToken>()))
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
    public async Task Handle_ValidRequest_ClosesAuction()
    {
        // Arrange
        var request = new CloseAuctionCommand("identifier");

        var vehicle = new VehicleBuilder().WithOpenAuction().Build();

        _vehicleRepositoryMock
            .Setup(repo => repo.GetVehicleWithOpenAuctionsAsync(request.VehicleIdentifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _vehicleRepositoryMock.Verify();
        result.IsSuccess.Should().BeTrue();
        vehicle.Auctions.Single().Status.Should().Be(AuctionStatus.Closed);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
    }
}
