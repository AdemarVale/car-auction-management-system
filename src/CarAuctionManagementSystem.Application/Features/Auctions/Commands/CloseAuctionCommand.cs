using FluentResults;
using MediatR;

namespace CarAuctionManagementSystem.Application.Features.Auctions.Commands;
public class CloseAuctionCommand : IRequest<Result>
{
    public string VehicleIdentifier { get; }

    public CloseAuctionCommand(string vehicleIdentifier)
    {
        VehicleIdentifier = vehicleIdentifier;
    }
}
