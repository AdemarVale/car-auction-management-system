using FluentResults;
using MediatR;

namespace CarAuctionManagementSystem.Application.Features.Bids.Commands;

public class CreateBidCommand : IRequest<Result>
{
    public string VehicleIdentifier { get; }

    public double Value { get; }

    public string UserIdentifier { get; }

    public CreateBidCommand(string vehicleIdentifier, double value, string userIdentifier)
    {
        VehicleIdentifier = vehicleIdentifier;
        Value = value;
        UserIdentifier = userIdentifier;
    }
}
