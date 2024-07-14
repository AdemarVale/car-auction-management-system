using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using CarAuctionManagementSystem.Api.Endpoints.V1.Contracts.Requests;
using CarAuctionManagementSystem.Api.Endpoints.V1.Contracts.Responses;
using CarAuctionManagementSystem.Application.Common.Dtos;
using CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Dtos;
using CarAuctionManagementSystem.Domain.Auctions;
using CarAuctionManagementSystem.Domain.Vehicles;
using CarAuctionManagementSystem.IntegrationTests.Fixtures;
using CarAuctionManagementSystem.Utilities.Builders;
using FluentAssertions;
using Xunit;

namespace CarAuctionManagementSystem.IntegrationTests.Tests.Api;

[Collection(InfrastructureCollection.Name)]
public class VehicleApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly DatabaseFixture _databaseFixture;

    public VehicleApiTests(CustomWebApplicationFactory factory, DatabaseFixture databaseFixture)
    {
        _factory = factory;
        _databaseFixture = databaseFixture;
    }

    [Fact]
    public async Task PostSedan_Success_CreatesNewVehicle()
    {
        // Arrange
        var client = _factory.CreateClient();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        await _databaseFixture.DeleteDataAsync();

        var identifier = "uniqueidentifier1";
        var request = new CreateVehicleRequest("sedan", identifier, "man", "mod", 2024, 5000, 5, null, null);

        var json = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var response = await client.PostAsync($"/api/v1/vehicles", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var vehicle = _databaseFixture.GetVehicleByIdentifier(identifier);
        vehicle.Should().NotBeNull();
        vehicle.Should().BeOfType<Sedan>();

        var sedan = (Sedan)vehicle!;
        sedan.Manufacturer.Should().Be(request.Manufacturer);
        sedan.Model.Should().Be(request.Model);
        sedan.Year.Should().Be(request.Year);
        sedan.StartingBid.Should().Be(request.StartingBid);
        sedan.NumberOfDoors.Should().Be(request.NumberOfDoors);
    }

    [Fact]
    public async Task PostTruck_Success_CreatesNewVehicle()
    {
        // Arrange
        var client = _factory.CreateClient();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        await _databaseFixture.DeleteDataAsync();

        var identifier = "uniqueidentifier1";
        var request = new CreateVehicleRequest("truck", identifier, "man", "mod", 2024, 5000, null, null, 500.0);

        var json = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var response = await client.PostAsync($"/api/v1/vehicles", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var vehicle = _databaseFixture.GetVehicleByIdentifier(identifier);
        vehicle.Should().NotBeNull();
        vehicle.Should().BeOfType<Truck>();

        var sedan = (Truck)vehicle!;
        sedan.Manufacturer.Should().Be(request.Manufacturer);
        sedan.Model.Should().Be(request.Model);
        sedan.Year.Should().Be(request.Year);
        sedan.StartingBid.Should().Be(request.StartingBid);
        sedan.LoadCapacity.Should().Be(request.LoadCapacity);
    }

    [Fact]
    public async Task PostSuv_Success_CreatesNewVehicle()
    {
        // Arrange
        var client = _factory.CreateClient();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        await _databaseFixture.DeleteDataAsync();

        var identifier = "uniqueidentifier1";
        var request = new CreateVehicleRequest("suv", identifier, "man", "mod", 2024, 5000, null, 10, null);

        var json = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var response = await client.PostAsync($"/api/v1/vehicles", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var vehicle = _databaseFixture.GetVehicleByIdentifier(identifier);
        vehicle.Should().NotBeNull();
        vehicle.Should().BeOfType<Suv>();

        var sedan = (Suv)vehicle!;
        sedan.Manufacturer.Should().Be(request.Manufacturer);
        sedan.Model.Should().Be(request.Model);
        sedan.Year.Should().Be(request.Year);
        sedan.StartingBid.Should().Be(request.StartingBid);
        sedan.NumberOfSeats.Should().Be(request.NumberOfSeats);
    }

    [Fact]
    public async Task PostHatchback_Success_CreatesNewVehicle()
    {
        // Arrange
        var client = _factory.CreateClient();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        await _databaseFixture.DeleteDataAsync();

        var identifier = "uniqueidentifier1";
        var request = new CreateVehicleRequest("hatchback", identifier, "man", "mod", 2024, 5000, 3, null, null);

        var json = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var response = await client.PostAsync($"/api/v1/vehicles", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var vehicle = _databaseFixture.GetVehicleByIdentifier(identifier);
        vehicle.Should().NotBeNull();
        vehicle.Should().BeOfType<Hatchback>();

        var sedan = (Hatchback)vehicle!;
        sedan.Manufacturer.Should().Be(request.Manufacturer);
        sedan.Model.Should().Be(request.Model);
        sedan.Year.Should().Be(request.Year);
        sedan.StartingBid.Should().Be(request.StartingBid);
        sedan.NumberOfDoors.Should().Be(request.NumberOfDoors);
    }

    [Fact]
    public async Task PostVehicle_RepeatedUniqueIdentifier_ReturnsError()
    {
        // Arrange
        var client = _factory.CreateClient();

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var vehicle = new VehicleBuilder().Build();

        await _databaseFixture.DeleteDataAsync();
        await _databaseFixture.CreateNewVehicle(vehicle);

        var request = new CreateVehicleRequest("hatchback", vehicle.Identifier, "man", "mod", 2024, 5000, 3, null, null);

        var json = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var response = await client.PostAsync($"/api/v1/vehicles", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        errorResponse!.Errors.Count().Should().Be(1);
        errorResponse!.Errors.First().Value.Count.Should().Be(1);
        errorResponse!.Errors.First().Value.First().Should().Be("Repeated vehicle identifier.");
    }

    [Fact]
    public async Task PostVehicle_ValidatorError_ReturnsError()
    {
        // Arrange
        var client = _factory.CreateClient();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        await _databaseFixture.DeleteDataAsync();

        var identifier = "id1";
        var request = new CreateVehicleRequest("hatchback", identifier, "", "mod", 2024, 5000, 3, null, null);

        var json = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var response = await client.PostAsync($"/api/v1/vehicles", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        errorResponse!.Errors.Count().Should().Be(1);
        errorResponse!.Errors.First().Value.Count.Should().Be(1);
        errorResponse!.Errors.First().Value.First().Should().Be("Invalid manufacturer.");
    }

    [Fact]
    public async Task GetVehicles_MatchSearchingCriteria_ReturnsVehicle()
    {
        // Arrange
        var client = _factory.CreateClient();

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var vehicle = new VehicleBuilder().Build();

        await _databaseFixture.DeleteDataAsync();
        await _databaseFixture.CreateNewVehicle(vehicle);

        // Act
        var response = await client.GetAsync(
            $"/api/v1/vehicles?pageNumber=1&pageSize=10&year={vehicle.Year}&model={vehicle.Model}&type={vehicle.GetType().Name}&manufacturer={vehicle.Manufacturer}",
            timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<VehicleDto>>();
        pagedResult!.TotalResultsCount.Should().Be(1);
        pagedResult.TotalPages.Should().Be(1);
        pagedResult.Results.Count().Should().Be(1);
    }

    [Fact]
    public async Task GetVehicles_NoMatchesOnSearchingCriteria_ReturnsEmpty()
    {
        // Arrange
        var client = _factory.CreateClient();

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var vehicle = new VehicleBuilder().Build();

        await _databaseFixture.DeleteDataAsync();
        await _databaseFixture.CreateNewVehicle(vehicle);

        // Act
        var response = await client.GetAsync(
            $"/api/v1/vehicles?pageNumber=1&pageSize=10&year={vehicle.Year + 1}",
            timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<VehicleDto>>();
        pagedResult!.TotalResultsCount.Should().Be(0);
        pagedResult.TotalPages.Should().Be(0);
        pagedResult.Results.Count().Should().Be(0);
    }

    [Fact]
    public async Task StartAuction_Success_Returns200OK()
    {
        // Arrange
        var client = _factory.CreateClient();

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var vehicle = new VehicleBuilder().Build();

        await _databaseFixture.DeleteDataAsync();
        await _databaseFixture.CreateNewVehicle(vehicle);

        var json = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);

        // Act
        var response = await client.PostAsync($"/api/v1/vehicles/{vehicle.Identifier}/auctions", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var newVehicle = _databaseFixture.GetVehicleByIdentifier(vehicle.Identifier);
        newVehicle!.Auctions.Count().Should().Be(1);
        newVehicle!.Auctions.Single().Status.Should().Be(AuctionStatus.Open);
    }

    [Fact]
    public async Task StartAuction_AuctionAlreadyOpen_Returns400BadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var vehicle = new VehicleBuilder().WithOpenAuction().Build();

        await _databaseFixture.DeleteDataAsync();
        await _databaseFixture.CreateNewVehicle(vehicle);

        var json = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);

        // Act
        var response = await client.PostAsync($"/api/v1/vehicles/{vehicle.Identifier}/auctions", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        errorResponse!.Errors.Count().Should().Be(1);
        errorResponse!.Errors.First().Value.Count.Should().Be(1);
        errorResponse!.Errors.First().Value.First().Should().Be("There is already an open auction.");
    }

    [Fact]
    public async Task CloseAuction_Success_Returns200OK()
    {
        // Arrange
        var client = _factory.CreateClient();

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var vehicle = new VehicleBuilder().WithOpenAuction().Build();

        await _databaseFixture.DeleteDataAsync();
        await _databaseFixture.CreateNewVehicle(vehicle);

        var json = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);

        // Act
        var response = await client.PutAsync($"/api/v1/vehicles/{vehicle.Identifier}/auctions/close", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var newVehicle = _databaseFixture.GetVehicleByIdentifier(vehicle.Identifier);
        newVehicle!.Auctions.Count().Should().Be(1);
        newVehicle!.Auctions.Single().Status.Should().Be(AuctionStatus.Closed);
    }

    [Fact]
    public async Task CloseAuction_NoOpenAuction_Returns400BadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var vehicle = new VehicleBuilder().Build();

        await _databaseFixture.DeleteDataAsync();
        await _databaseFixture.CreateNewVehicle(vehicle);

        var json = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);

        // Act
        var response = await client.PutAsync($"/api/v1/vehicles/{vehicle.Identifier}/auctions/close", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        errorResponse!.Errors.Count().Should().Be(1);
        errorResponse!.Errors.First().Value.Count.Should().Be(1);
        errorResponse!.Errors.First().Value.First().Should().Be("There are no open auctions.");
    }

    [Fact]
    public async Task CreateBid_Success_Returns200OK()
    {
        // Arrange
        var client = _factory.CreateClient();

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var vehicle = new VehicleBuilder().WithOpenAuction().Build();

        await _databaseFixture.DeleteDataAsync();
        await _databaseFixture.CreateNewVehicle(vehicle);

        var request = new CreateBidRequest(10000, "user1");

        var json = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var response = await client.PostAsync($"/api/v1/vehicles/{vehicle.Identifier}/auctions/bids", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var newVehicle = _databaseFixture.GetVehicleByIdentifier(vehicle.Identifier);
        newVehicle!.Auctions.Count().Should().Be(1);
        newVehicle!.Auctions.Single().Status.Should().Be(AuctionStatus.Open);
        newVehicle!.Auctions.Single().Bids.Count.Should().Be(1);
        newVehicle!.Auctions.Single().Bids.Single().Value.Should().Be(request.Value);
    }

    [Fact]
    public async Task CreateBid_NoOpenAuction_Returns400BadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var vehicle = new VehicleBuilder().Build();

        await _databaseFixture.DeleteDataAsync();
        await _databaseFixture.CreateNewVehicle(vehicle);

        var request = new CreateBidRequest(10000, "user1");

        var json = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var response = await client.PostAsync($"/api/v1/vehicles/{vehicle.Identifier}/auctions/bids", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        errorResponse!.Errors.Count().Should().Be(1);
        errorResponse!.Errors.First().Value.Count.Should().Be(1);
        errorResponse!.Errors.First().Value.First().Should().Be("There are no open auctions.");

        var newVehicle = _databaseFixture.GetVehicleByIdentifier(vehicle.Identifier);
        newVehicle!.Auctions.Count().Should().Be(0);
    }

    [Fact]
    public async Task CreateBid_NotBiggerThanStartingBid_Returns400BadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var vehicle = new VehicleBuilder().WithOpenAuction().Build();

        await _databaseFixture.DeleteDataAsync();
        await _databaseFixture.CreateNewVehicle(vehicle);

        var request = new CreateBidRequest(5000, "user1");

        var json = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var response = await client.PostAsync($"/api/v1/vehicles/{vehicle.Identifier}/auctions/bids", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        errorResponse!.Errors.Count().Should().Be(1);
        errorResponse!.Errors.First().Value.Count.Should().Be(1);
        errorResponse!.Errors.First().Value.First().Should().Be("There is already a bigger or equal bid for this vehicle.");

        var newVehicle = _databaseFixture.GetVehicleByIdentifier(vehicle.Identifier);
        newVehicle!.Auctions.Count().Should().Be(1);
        newVehicle!.Auctions.Single().Status.Should().Be(AuctionStatus.Open);
        newVehicle!.Auctions.Single().Bids.Count.Should().Be(0);
    }

    [Fact]
    public async Task CreateBid_NotBiggerThanLastBid_Returns400BadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var vehicle = new VehicleBuilder().WithBid(7000).Build();

        await _databaseFixture.DeleteDataAsync();
        await _databaseFixture.CreateNewVehicle(vehicle);

        var request = new CreateBidRequest(6000, "user1");

        var json = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        var response = await client.PostAsync($"/api/v1/vehicles/{vehicle.Identifier}/auctions/bids", json, timeout.Token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        errorResponse!.Errors.Count().Should().Be(1);
        errorResponse!.Errors.First().Value.Count.Should().Be(1);
        errorResponse!.Errors.First().Value.First().Should().Be("There is already a bigger or equal bid for this vehicle.");

        var newVehicle = _databaseFixture.GetVehicleByIdentifier(vehicle.Identifier);
        newVehicle!.Auctions.Count().Should().Be(1);
        newVehicle!.Auctions.Single().Status.Should().Be(AuctionStatus.Open);
        newVehicle!.Auctions.Single().Bids.Count.Should().Be(1);
        newVehicle!.Auctions.Single().Bids.Single().Value.Should().Be(7000);
    }
}
