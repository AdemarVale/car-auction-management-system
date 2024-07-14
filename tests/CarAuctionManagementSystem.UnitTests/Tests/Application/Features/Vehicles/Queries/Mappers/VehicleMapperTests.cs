using CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Mappers;
using CarAuctionManagementSystem.Domain.Vehicles;
using CarAuctionManagementSystem.Utilities.Builders;
using FluentAssertions;
using Xunit;

namespace CarAuctionManagementSystem.UnitTests.Tests.Application.Features.Vehicles.Queries.Mappers;

public class VehicleMapperTests
{
    [Fact]
    public void MapToDto_SedanWithBids_ShouldMapCorrectly()
    {
        // Arrange
        var sedan = new VehicleBuilder().WithBid(5500).Build();

        // Act
        var dto = sedan.MapToDto();

        // Assert
        dto.Type.Should().Be(nameof(Sedan));
        dto.NumberOfSeats.Should().BeNull();
        dto.LoadCapacity.Should().BeNull();
        dto.NumberOfDoors.Should().Be(sedan.NumberOfDoors);
        dto.Identifier.Should().Be(sedan.Identifier);
        dto.Manufacturer.Should().Be(sedan.Manufacturer);
        dto.Model.Should().Be(sedan.Model);
        dto.Year.Should().Be(sedan.Year);
        dto.StartingBid.Should().Be(sedan.StartingBid);

        dto.ActiveAuction.Should().NotBeNull();
        dto.ActiveAuction!.CreatedAt.Should().Be(sedan.Auctions.Single().CreatedAt);
        dto.ActiveAuction.BiggestBid.Should().NotBeNull();
        dto.ActiveAuction.BiggestBid!.CreatedAt.Should().Be(sedan.Auctions.Single().Bids.Single().CreatedAt);
        dto.ActiveAuction.BiggestBid!.Value.Should().Be(sedan.Auctions.Single().Bids.Single().Value);
        dto.ActiveAuction.BiggestBid!.UserIdentifier.Should().Be(sedan.Auctions.Single().Bids.Single().UserIdentifier);
    }

    [Fact]
    public void MapToDto_SedanWithOpenAuctionNoBids_ShouldMapCorrectly()
    {
        // Arrange
        var sedan = new VehicleBuilder().WithOpenAuction().Build();

        // Act
        var dto = sedan.MapToDto();

        // Assert
        dto.Type.Should().Be(nameof(Sedan));
        dto.NumberOfSeats.Should().BeNull();
        dto.LoadCapacity.Should().BeNull();
        dto.NumberOfDoors.Should().Be(sedan.NumberOfDoors);
        dto.Identifier.Should().Be(sedan.Identifier);
        dto.Manufacturer.Should().Be(sedan.Manufacturer);
        dto.Model.Should().Be(sedan.Model);
        dto.Year.Should().Be(sedan.Year);
        dto.StartingBid.Should().Be(sedan.StartingBid);

        dto.ActiveAuction.Should().NotBeNull();
        dto.ActiveAuction!.CreatedAt.Should().Be(sedan.Auctions.Single().CreatedAt);
        dto.ActiveAuction.BiggestBid.Should().BeNull();
    }

    [Fact]
    public void MapToDto_IsSuv_ShouldMapCorrectly()
    {
        // Arrange
        var suv = new Suv(5, "identifier", "manufacturer", "model", 2024, 5000);

        // Act
        var dto = suv.MapToDto();

        // Assert
        dto.Type.Should().Be(nameof(Suv));
        dto.NumberOfDoors.Should().BeNull();
        dto.LoadCapacity.Should().BeNull();
        dto.NumberOfSeats.Should().Be(suv.NumberOfSeats);
        dto.Identifier.Should().Be(suv.Identifier);
        dto.Manufacturer.Should().Be(suv.Manufacturer);
        dto.Model.Should().Be(suv.Model);
        dto.Year.Should().Be(suv.Year);
        dto.StartingBid.Should().Be(suv.StartingBid);
    }

    [Fact]
    public void MapToDto_IsHatchback_ShouldMapCorrectly()
    {
        // Arrange
        var hatchback = new Hatchback(5, "identifier", "manufacturer", "model", 2024, 5000);

        // Act
        var dto = hatchback.MapToDto();

        // Assert
        dto.Type.Should().Be(nameof(Hatchback));
        dto.NumberOfSeats.Should().BeNull();
        dto.LoadCapacity.Should().BeNull();
        dto.NumberOfDoors.Should().Be(hatchback.NumberOfDoors);
        dto.Identifier.Should().Be(hatchback.Identifier);
        dto.Manufacturer.Should().Be(hatchback.Manufacturer);
        dto.Model.Should().Be(hatchback.Model);
        dto.Year.Should().Be(hatchback.Year);
        dto.StartingBid.Should().Be(hatchback.StartingBid);
    }

    [Fact]
    public void MapToDto_IsTruck_ShouldMapCorrectly()
    {
        // Arrange
        var truck = new Truck(5, "identifier", "manufacturer", "model", 2024, 5000);

        // Act
        var dto = truck.MapToDto();

        // Assert
        dto.Type.Should().Be(nameof(Truck));
        dto.NumberOfSeats.Should().BeNull();
        dto.NumberOfDoors.Should().BeNull();
        dto.LoadCapacity.Should().Be(truck.LoadCapacity);
        dto.Identifier.Should().Be(truck.Identifier);
        dto.Manufacturer.Should().Be(truck.Manufacturer);
        dto.Model.Should().Be(truck.Model);
        dto.Year.Should().Be(truck.Year);
        dto.StartingBid.Should().Be(truck.StartingBid);
    }
}
