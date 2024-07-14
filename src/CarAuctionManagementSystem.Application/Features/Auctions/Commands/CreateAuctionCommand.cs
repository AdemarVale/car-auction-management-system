using FluentResults;
using MediatR;

namespace CarAuctionManagementSystem.Application.Features.Auctions.Commands;
public class CreateAuctionCommand : IRequest<Result>
{
    public string VehicleIdentifier { get; }

    public CreateAuctionCommand(string vehicleIdentifier)
    {
        VehicleIdentifier = vehicleIdentifier;
    }
}
