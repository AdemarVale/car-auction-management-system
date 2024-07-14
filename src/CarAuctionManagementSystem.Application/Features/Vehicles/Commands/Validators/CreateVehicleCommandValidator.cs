using CarAuctionManagementSystem.Domain.Vehicles;
using FluentValidation;

namespace CarAuctionManagementSystem.Application.Features.Vehicles.Commands.Validators;
public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(x => x.Type)
            .Must(x => Enum.TryParse<VehicleType>(x, true, out _))
            .WithMessage("Invalid type.");

        RuleFor(x => x.Identifier)
            .NotEmpty()
            .WithMessage("Invalid identifier.");

        RuleFor(x => x.Manufacturer)
            .NotEmpty()
            .WithMessage("Invalid manufacturer.");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Invalid model.");

        RuleFor(x => x.Year)
            .NotEmpty()
            .WithMessage("Invalid year.");

        RuleFor(x => x.StartingBid)
            .NotEmpty()
            .WithMessage("Invalid starting bid.");

        RuleFor(x => x.NumberOfDoors)
            .Must(x => x > 0)
            .When(x => Enum.TryParse<VehicleType>(x.Type, true, out var type) && (type == VehicleType.Sedan || type == VehicleType.Hatchback))
            .WithMessage("Invalid number of doors for selected type.");

        RuleFor(x => x.NumberOfSeats)
            .Must(x => x > 0)
            .When(x => Enum.TryParse<VehicleType>(x.Type, true, out var type) && type == VehicleType.Suv)
            .WithMessage("Invalid number of seats for selected type.");

        RuleFor(x => x.LoadCapacity)
            .Must(x => x > 0)
            .When(x => Enum.TryParse<VehicleType>(x.Type, true, out var type) && type == VehicleType.Truck)
            .WithMessage("Invalid load capacity for selected type.");
    }
}
