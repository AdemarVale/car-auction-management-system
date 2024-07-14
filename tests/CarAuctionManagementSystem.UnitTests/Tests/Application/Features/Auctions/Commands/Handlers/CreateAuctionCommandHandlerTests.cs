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

public class CreateAuctionCommandHandlerTests
{
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CreateAuctionCommandHandler>> _loggerMock;

    private readonly CreateAuctionCommandHandler _handler;

    public CreateAuctionCommandHandlerTests()
    {
        _vehicleRepositoryMock = new Mock<IVehicleRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<CreateAuctionCommandHandler>>();

        _handler = new CreateAuctionCommandHandler(_vehicleRepositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_VehicleNotFound_ReturnsError()
    {
        // Arrange
        var request = new CreateAuctionCommand("identifier");

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
    public async Task Handle_WithOpenAuctions_ReturnsError()
    {
        // Arrange
        var request = new CreateAuctionCommand("identifier");

        var vehicle = new VehicleBuilder().WithOpenAuction().Build();

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
        result.Errors[0].Message.Should().Be("There is already an open auction.");
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesAuction()
    {
        // Arrange
        var request = new CreateAuctionCommand("identifier");

        var vehicle = new VehicleBuilder().Build();

        _vehicleRepositoryMock
            .Setup(repo => repo.GetVehicleWithOpenAuctionsAsync(request.VehicleIdentifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vehicle);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _vehicleRepositoryMock.Verify();
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));

        result.IsSuccess.Should().BeTrue();
        vehicle.Auctions.Single().Status.Should().Be(AuctionStatus.Open);
    }
}
