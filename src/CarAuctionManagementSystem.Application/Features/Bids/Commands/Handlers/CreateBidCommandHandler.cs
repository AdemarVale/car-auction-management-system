using CarAuctionManagementSystem.Application.Common.Abstractions;
using CarAuctionManagementSystem.Application.Common.Mappers;
using CarAuctionManagementSystem.Application.Features.Bids.Commands.Validators;
using CarAuctionManagementSystem.Domain.Auctions;
using CarAuctionManagementSystem.Domain.Bids;
using CarAuctionManagementSystem.Domain.Vehicles;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarAuctionManagementSystem.Application.Features.Bids.Commands.Handlers;
public class CreateBidCommandHandler : IRequestHandler<CreateBidCommand, Result>
{
    private readonly CreateBidCommandValidator _validator;

    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateBidCommandHandler> _logger;

    public CreateBidCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, ILogger<CreateBidCommandHandler> logger)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = new();
    }

    public async Task<Result> Handle(CreateBidCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return validationResult.MapToFailedResult();
        }

        var vehicle = await _vehicleRepository.GetVehicleWithBiggestBidAsync(request.VehicleIdentifier, cancellationToken);

        if (vehicle is null)
        {
            return new Error("Vehicle not found.").CausedBy("VehicleIdentifier");
        }

        if (vehicle.Auctions.Count == 0)
        {
            return new Error("There are no open auctions.").CausedBy("VehicleIdentifier");
        }

        var auction = vehicle.Auctions.Single();

        if (NewBidValueIsNotBigEnough(request, vehicle, auction))
        {
            return new Error("There is already a bigger or equal bid for this vehicle.").CausedBy("Value");
        }

        auction.Bids.Add(new Bid(request.Value, request.UserIdentifier));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "User '{userIdentifier}' has placed a new bid with value '{value}' for vehicle '{vehicleIdentifier}'.",
            request.UserIdentifier,
            request.Value,
            request.VehicleIdentifier);

        return Result.Ok();
    }

    private static bool NewBidValueIsNotBigEnough(CreateBidCommand request, Vehicle vehicle, Auction auction)
    {
        return vehicle.StartingBid >= request.Value || (auction.Bids.Count > 0 && auction.Bids.Single().Value >= request.Value);
    }
}
