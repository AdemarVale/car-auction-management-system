using FluentValidation;

namespace CarAuctionManagementSystem.Application.Features.Bids.Commands.Validators;

public class CreateBidCommandValidator : AbstractValidator<CreateBidCommand>
{
    public CreateBidCommandValidator()
    {
        {
            RuleFor(x => x.VehicleIdentifier)
                .NotEmpty()
                .WithMessage("Invalid vehicle identifier.");

            RuleFor(x => x.Value)
                .NotEmpty()
                .WithMessage("Invalid value.");

            RuleFor(x => x.UserIdentifier)
                .NotEmpty()
                .WithMessage("Invalid user identifier.");
        }
    }
}
