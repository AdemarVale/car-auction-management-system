using CarAuctionManagementSystem.Application.Common.Abstractions;
using CarAuctionManagementSystem.Application.Features.Vehicles.Commands;
using CarAuctionManagementSystem.Application.Features.Vehicles.Commands.Handlers;
using CarAuctionManagementSystem.Domain.Vehicles;
using FluentAssertions;
using Moq;
using Xunit;

namespace CarAuctionManagementSystem.UnitTests.Tests.Application.Features.Vehicles.Commands.Handlers;
public class CreateVehicleCommandHandlerTests
{
    private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly CreatevehicleCommandHandler _handler;

    public CreateVehicleCommandHandlerTests()
    {
        _vehicleRepositoryMock = new Mock<IVehicleRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new CreatevehicleCommandHandler(_vehicleRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidationsFails_ShouldReturnValidationError()
    {
        // Arrange
        var command = new CreateVehicleCommand("type", "identifier", "manufacturer", "model", 200, 200, null, null, null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Count().Should().Be(1);
        result.Errors[0].Message.Should().Be("Invalid type.");
    }

    [Fact]
    public async Task Handle_VehicleIsNotUnique_ShouldReturnError()
    {
        // Arrange
        var command = new CreateVehicleCommand("sedan", "identifier", "manufacturer", "model", 200, 200, 5, null, null);

        _vehicleRepositoryMock
            .Setup(repo => repo.IsUniqueAsync(command.Identifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _vehicleRepositoryMock.Verify();
        result.IsFailed.Should().BeTrue();
        result.Errors.Count().Should().Be(1);
        result.Errors[0].Message.Should().Be("Repeated vehicle identifier.");
    }

    [Theory]
    [InlineData("sedan")]
    [InlineData("truck")]
    [InlineData("hatchback")]
    [InlineData("suv")]
    public async Task Handle_ShouldAddVehicle_WhenCommandIsValid(string type)
    {
        // Arrange
        var command = new CreateVehicleCommand(type, "identifier", "manufacturer", "model", 2014, 5500, 5, 5, 5);

        _vehicleRepositoryMock
            .Setup(repo => repo.IsUniqueAsync(command.Identifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _vehicleRepositoryMock.Verify();

        _vehicleRepositoryMock
            .Verify(
                x => x.AddAsync(
                    It.Is<Vehicle>(
                        x => string.Equals(type, x.GetType().Name, StringComparison.InvariantCultureIgnoreCase) &&
                        x.Identifier == command.Identifier &&
                        x.Manufacturer == command.Manufacturer &&
                        x.Model == command.Model &&
                        x.Year == command.Year &&
                        x.StartingBid == command.StartingBid),
                    It.IsAny<CancellationToken>()),
                Times.Once);

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.IsSuccess.Should().BeTrue();
    }
}
