using System.ComponentModel;
using CarAuctionManagementSystem.Application.Common.Abstractions;
using CarAuctionManagementSystem.Application.Common.Mappers;
using CarAuctionManagementSystem.Application.Features.Vehicles.Commands.Validators;
using CarAuctionManagementSystem.Domain.Vehicles;
using FluentResults;
using MediatR;

namespace CarAuctionManagementSystem.Application.Features.Vehicles.Commands.Handlers;
public class CreatevehicleCommandHandler : IRequestHandler<CreateVehicleCommand, Result>
{
    private readonly CreateVehicleCommandValidator _validator;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatevehicleCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _validator = new();
    }

    public async Task<Result> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return validationResult.MapToFailedResult();
        }

        var isUnique = await _vehicleRepository.IsUniqueAsync(request.Identifier, cancellationToken);

        if (!isUnique)
        {
            return new Error("Repeated vehicle identifier.").CausedBy("Identifier");
        }

        var vehicle = CreatevehicleByType(request);

        await _vehicleRepository.AddAsync(vehicle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    private static Vehicle CreatevehicleByType(CreateVehicleCommand request)
    {
        var type = Enum.Parse<VehicleType>(request.Type, true);

        return type switch
        {
            VehicleType.Sedan => new Sedan(request.NumberOfDoors!.Value, request.Identifier, request.Manufacturer, request.Model, request.Year, request.StartingBid),
            VehicleType.Truck => new Truck(request.LoadCapacity!.Value, request.Identifier, request.Manufacturer, request.Model, request.Year, request.StartingBid),
            VehicleType.Hatchback => new Hatchback(request.NumberOfDoors!.Value, request.Identifier, request.Manufacturer, request.Model, request.Year, request.StartingBid),
            VehicleType.Suv => new Suv(request.NumberOfSeats!.Value, request.Identifier, request.Manufacturer, request.Model, request.Year, request.StartingBid),
            _ => throw new InvalidEnumArgumentException(nameof(type)),
        };
    }
}
