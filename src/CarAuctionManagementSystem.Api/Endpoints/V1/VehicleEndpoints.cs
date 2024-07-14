using CarAuctionManagementSystem.Api.Endpoints.V1.Contracts.Requests;
using CarAuctionManagementSystem.Api.Endpoints.V1.Contracts.Responses;
using CarAuctionManagementSystem.Api.Endpoints.V1.Contracts.Responses.Mapper;
using CarAuctionManagementSystem.Application.Common.Dtos;
using CarAuctionManagementSystem.Application.Features.Auctions.Commands;
using CarAuctionManagementSystem.Application.Features.Bids.Commands;
using CarAuctionManagementSystem.Application.Features.Vehicles.Commands;
using CarAuctionManagementSystem.Application.Features.Vehicles.Queries;
using CarAuctionManagementSystem.Application.Features.Vehicles.Queries.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CarAuctionManagementSystem.Api.Endpoints.V1;

public static class VehicleEndpoints
{
    public static void MapvVhicleEndpoints(this IEndpointRouteBuilder application)
    {
        var endpointsGroup = application
            .MapGroup("/vehicles")
            .MapToApiVersion(1);

        endpointsGroup
            .MapPost("/", CreateVehicleAsync)
            .WithName("CreateVehicle");

        endpointsGroup
            .MapGet("/", GetVehiclesAsync)
            .WithName("GetVehicles");

        endpointsGroup
            .MapPost("{identifier}/auctions", StartAuctionAsync)
            .WithName("CreateAuction");

        endpointsGroup
            .MapPost("{identifier}/auctions/bids", CreateBidAsync)
            .WithName("CreateBid");

        endpointsGroup
            .MapPut("{identifier}/auctions/close", CloseAuctionAsync)
            .WithName("CloseAuction");
    }

    public static async Task<Results<Ok, BadRequest<ErrorResponse>>> CreateVehicleAsync(
        [FromBody] CreateVehicleRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new CreateVehicleCommand(
                type: request.Type,
                identifier: request.Identifier,
                manufacturer: request.Manufacturer,
                model: request.Model,
                year: request.Year,
                startingBid: request.StartingBid,
                numberOfDoors: request.NumberOfDoors,
                numberOfSeats: request.NumberOfSeats,
                loadCapacity: request.LoadCapacity),
            cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        return TypedResults.BadRequest(result.Errors.MapToErrorResponse(400));
    }

    public static async Task<Ok<PagedResult<VehicleDto>>> GetVehiclesAsync(
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize,
        [FromQuery] string? type,
        [FromQuery] string? model,
        [FromQuery] string? manufacturer,
        [FromQuery] int? year,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetVehiclesQuery(
                pageNumber: pageNumber,
                pageSize: pageSize,
                type: type,
                manufacturer: manufacturer,
                model: model,
                year: year),
            cancellationToken);

        return TypedResults.Ok(result);
    }

    public static async Task<Results<Ok, BadRequest<ErrorResponse>>> StartAuctionAsync(
        [FromRoute] string identifier,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateAuctionCommand(identifier), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        return TypedResults.BadRequest(result.Errors.MapToErrorResponse(400));
    }

    public static async Task<Results<Ok, BadRequest<ErrorResponse>>> CreateBidAsync(
        [FromRoute] string identifier,
        [FromBody] CreateBidRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateBidCommand(identifier, request.Value, request.UserIdentifier), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        return TypedResults.BadRequest(result.Errors.MapToErrorResponse(400));
    }

    public static async Task<Results<Ok, BadRequest<ErrorResponse>>> CloseAuctionAsync(
        [FromRoute] string identifier,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CloseAuctionCommand(identifier), cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok();
        }

        return TypedResults.BadRequest(result.Errors.MapToErrorResponse(400));
    }
}
