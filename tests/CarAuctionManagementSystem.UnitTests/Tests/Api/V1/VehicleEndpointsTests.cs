using AutoFixture;
using CarAuctionManagementSystem.Api.Endpoints.V1;
using CarAuctionManagementSystem.Api.Endpoints.V1.Contracts.Requests;
using CarAuctionManagementSystem.Api.Endpoints.V1.Contracts.Responses;
using CarAuctionManagementSystem.Application.Common.Dtos;
using CarAuctionManagementSystem.Application.Features.Auctions.Commands;
using CarAuctionManagementSystem.Application.Features.Bids.Commands;
using CarAuctionManagementSystem.Application.Features.Vehicles.Commands;
using CarAuctionManagementSystem.Application.Features.Vehicles.Queries;
using CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Dtos;
using FluentAssertions;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Xunit;

namespace CarAuctionManagementSystem.UnitTests.Tests.Api.V1;
public class VehicleEndpointsTests
{
    private readonly Mock<ISender> _sender;
    private readonly Fixture _fixture;

    public VehicleEndpointsTests()
    {
        _sender = new Mock<ISender>();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CreateVehicle_Success_Returns200Ok()
    {
        // Arrange
        var request = _fixture.Create<CreateVehicleRequest>();

        _sender
            .Setup(x => x.Send(
                It.Is<CreateVehicleCommand>(x =>
                    x.Type == request.Type &&
                    x.Identifier == request.Identifier &&
                    x.Manufacturer == request.Manufacturer &&
                    x.Model == request.Model &&
                    x.Year == request.Year &&
                    x.StartingBid == request.StartingBid &&
                    x.NumberOfDoors == request.NumberOfDoors &&
                    x.NumberOfSeats == request.NumberOfSeats &&
                    x.LoadCapacity == request.LoadCapacity),
                default))
            .ReturnsAsync(Result.Ok())
            .Verifiable();

        // Act
        var result = await VehicleEndpoints.CreateVehicleAsync(request, _sender.Object, default);

        // Assert
        _sender.Verify();
        result.Result.ToResult().IsSuccess.Should().BeTrue();
        result.Result.Should().BeOfType<Ok>();
    }

    [Fact]
    public async Task CreateVehicle_Fails_Returns400BadRequest()
    {
        // Arrange
        var request = _fixture.Create<CreateVehicleRequest>();

        _sender
            .Setup(x => x.Send(
                It.Is<CreateVehicleCommand>(x =>
                    x.Type == request.Type &&
                    x.Identifier == request.Identifier &&
                    x.Manufacturer == request.Manufacturer &&
                    x.Model == request.Model &&
                    x.Year == request.Year &&
                    x.StartingBid == request.StartingBid &&
                    x.NumberOfDoors == request.NumberOfDoors &&
                    x.NumberOfSeats == request.NumberOfSeats &&
                    x.LoadCapacity == request.LoadCapacity),
                default))
            .ReturnsAsync(Result.Fail(new Error("error").CausedBy("prop")))
            .Verifiable();

        // Act
        var result = await VehicleEndpoints.CreateVehicleAsync(request, _sender.Object, default);

        // Assert
        _sender.Verify();
        result.Result.ToResult().IsSuccess.Should().BeTrue();
        result.Result.Should().BeOfType<BadRequest<ErrorResponse>>();
    }

    [Fact]
    public async Task GetVehicles_Success_Returns200Ok()
    {
        // Arrange
        int? pageNumber = 1;
        int? pageSize = 10;
        int? year = 2004;

        var pagedResult = _fixture.Create<PagedResult<VehicleDto>>();

        _sender
            .Setup(x => x.Send(It.IsAny<GetVehiclesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult)
            .Verifiable();

        // Act
        var result = await VehicleEndpoints.GetVehiclesAsync(pageNumber, pageSize, null, null, null, year, _sender.Object, default);

        // Assert
        _sender.Verify();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(pagedResult);
    }

    [Fact]
    public async Task StartAuction_Success_Returns200Ok()
    {
        // Arrange
        var identifier = "identifier";

        _sender
            .Setup(x => x.Send(
                It.Is<CreateAuctionCommand>(x => x.VehicleIdentifier == identifier),
                default))
            .ReturnsAsync(Result.Ok())
            .Verifiable();

        // Act
        var result = await VehicleEndpoints.StartAuctionAsync(identifier, _sender.Object, default);

        // Assert
        _sender.Verify();
        result.Result.ToResult().IsSuccess.Should().BeTrue();
        result.Result.Should().BeOfType<Ok>();
    }

    [Fact]
    public async Task StartAuction_Fails_Returns400BadRequest()
    {
        // Arrange
        var identifier = "identifier";

        _sender
            .Setup(x => x.Send(
                It.Is<CreateAuctionCommand>(x => x.VehicleIdentifier == identifier),
                default))
            .ReturnsAsync(Result.Fail(new Error("error").CausedBy("prop")))
            .Verifiable();

        // Act
        var result = await VehicleEndpoints.StartAuctionAsync(identifier, _sender.Object, default);

        // Assert
        _sender.Verify();
        result.Result.ToResult().IsSuccess.Should().BeTrue();
        result.Result.Should().BeOfType<BadRequest<ErrorResponse>>();
    }

    [Fact]
    public async Task CloseAuction_Success_Returns200Ok()
    {
        // Arrange
        var identifier = "identifier";

        _sender
            .Setup(x => x.Send(
                It.Is<CloseAuctionCommand>(x => x.VehicleIdentifier == identifier),
                default))
            .ReturnsAsync(Result.Ok())
            .Verifiable();

        // Act
        var result = await VehicleEndpoints.CloseAuctionAsync(identifier, _sender.Object, default);

        // Assert
        _sender.Verify();
        result.Result.ToResult().IsSuccess.Should().BeTrue();
        result.Result.Should().BeOfType<Ok>();
    }

    [Fact]
    public async Task CloseAuction_Fails_Returns400BadRequest()
    {
        // Arrange
        var identifier = "identifier";

        _sender
            .Setup(x => x.Send(
                It.Is<CloseAuctionCommand>(x => x.VehicleIdentifier == identifier),
                default))
            .ReturnsAsync(Result.Fail(new Error("error").CausedBy("prop")))
            .Verifiable();

        // Act
        var result = await VehicleEndpoints.CloseAuctionAsync(identifier, _sender.Object, default);

        // Assert
        _sender.Verify();
        result.Result.ToResult().IsSuccess.Should().BeTrue();
        result.Result.Should().BeOfType<BadRequest<ErrorResponse>>();
    }

    [Fact]
    public async Task CreateBid_Success_Returns200Ok()
    {
        // Arrange
        var identifier = "identifier";
        var value = 50;
        var userIdentifier = "user";

        _sender
            .Setup(x => x.Send(
                It.Is<CreateBidCommand>(
                    x => x.VehicleIdentifier == identifier &&
                    x.Value == value &&
                    x.UserIdentifier == userIdentifier),
                default))
            .ReturnsAsync(Result.Ok())
            .Verifiable();

        // Act
        var result = await VehicleEndpoints.CreateBidAsync(identifier, new CreateBidRequest(value, userIdentifier), _sender.Object, default);

        // Assert
        _sender.Verify();
        result.Result.ToResult().IsSuccess.Should().BeTrue();
        result.Result.Should().BeOfType<Ok>();
    }

    [Fact]
    public async Task CreateBid_Fails_Returns200Ok()
    {
        // Arrange
        var identifier = "identifier";
        var value = 50;
        var userIdentifier = "user";

        _sender
            .Setup(x => x.Send(
                It.Is<CreateBidCommand>(
                    x => x.VehicleIdentifier == identifier &&
                    x.Value == value &&
                    x.UserIdentifier == userIdentifier),
                default))
            .ReturnsAsync(Result.Fail(new Error("error").CausedBy("prop")))
            .Verifiable();

        // Act
        var result = await VehicleEndpoints.CreateBidAsync(identifier, new CreateBidRequest(value, userIdentifier), _sender.Object, default);

        // Assert
        _sender.Verify();
        result.Result.ToResult().IsSuccess.Should().BeTrue();
        result.Result.Should().BeOfType<BadRequest<ErrorResponse>>();
    }
}
