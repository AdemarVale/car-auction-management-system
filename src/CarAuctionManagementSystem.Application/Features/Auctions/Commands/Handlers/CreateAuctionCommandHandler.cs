using CarAuctionManagementSystem.Application.Common.Abstractions;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarAuctionManagementSystem.Application.Features.Auctions.Commands.Handlers;
public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, Result>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateAuctionCommandHandler> _logger;

    public CreateAuctionCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, ILogger<CreateAuctionCommandHandler> logger)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetVehicleWithOpenAuctionsAsync(request.VehicleIdentifier, cancellationToken);

        if (vehicle is null)
        {
            return new Error("Vehicle not found.").CausedBy("VehicleIdentifier");
        }

        if (vehicle.Auctions.Count > 0)
        {
            return new Error("There is already an open auction.").CausedBy("VehicleIdentifier");
        }

        vehicle.StartAuction();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Auction started for vehicle '{identifier}'.", vehicle.Identifier);

        return Result.Ok();
    }
}