using CarAuctionManagementSystem.Application.Common.Abstractions;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarAuctionManagementSystem.Application.Features.Auctions.Commands.Handlers;
public class CloseAuctionCommandHandler : IRequestHandler<CloseAuctionCommand, Result>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CloseAuctionCommandHandler> _logger;

    public CloseAuctionCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, ILogger<CloseAuctionCommandHandler> logger)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(CloseAuctionCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetVehicleWithOpenAuctionsAsync(request.VehicleIdentifier, cancellationToken);

        if (vehicle is null)
        {
            return new Error("Vehicle not found.").CausedBy("VehicleIdentifier");
        }

        if (vehicle.Auctions.Count == 0)
        {
            return new Error("There are no open auctions.").CausedBy("VehicleIdentifier");
        }

        vehicle.Auctions.Single().Close();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Auction closed for vehicle '{vehicleIdentifier}'.", vehicle.Identifier);

        return Result.Ok();
    }
}
