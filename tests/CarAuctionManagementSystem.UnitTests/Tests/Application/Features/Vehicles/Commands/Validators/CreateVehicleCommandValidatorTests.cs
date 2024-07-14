using CarAuctionManagementSystem.Application.Features.Vehicles.Commands;
using CarAuctionManagementSystem.Application.Features.Vehicles.Commands.Validators;
using FluentAssertions;
using Xunit;

namespace CarAuctionManagementSystem.UnitTests.Tests.Application.Features.Vehicles.Commands.Validators;
public class CreateVehicleCommandValidatorTests
{
    private readonly CreateVehicleCommandValidator _validator;

    public CreateVehicleCommandValidatorTests()
    {
        _validator = new();
    }

    [Theory]
    [InlineData("sedan", "id", "man", "mod", 2000, 500, null, null, null, "Invalid number of doors for selected type.")]
    [InlineData("hatchback", "id", "man", "mod", 2000, 500, null, null, null, "Invalid number of doors for selected type.")]
    [InlineData("suv", "id", "man", "mod", 2000, 500, null, null, null, "Invalid number of seats for selected type.")]
    [InlineData("truck", "id", "man", "mod", 2000, 500, null, null, null, "Invalid load capacity for selected type.")]
    [InlineData("aaa", "id", "man", "mod", 2000, 500, null, null, null, "Invalid type.")]
    [InlineData("truck", "", "man", "mod", 2000, 500, null, null, 500.0, "Invalid identifier.")]
    [InlineData("truck", "a", "", "mod", 2000, 500, null, null, 500.0, "Invalid manufacturer.")]
    [InlineData("truck", "a", "a", "", 2000, 500, null, null, 500.0, "Invalid model.")]
    [InlineData("truck", "a", "a", "b", 0, 500, null, null, 500.0, "Invalid year.")]
    [InlineData("truck", "a", "a", "b", 500, 0, null, null, 500.0, "Invalid starting bid.")]
    public void Validate_ErrorFound_ShouldMatchExpectedError(
        string type,
        string identifier,
        string manufacturer,
        string model,
        int year,
        double startingBid,
        int? numberOfDoors,
        int? numberOfSeats,
        double? loadCapacity,
        string expectedError)
    {
        // Arrange
        var command = new CreateVehicleCommand(type, identifier, manufacturer, model, year, startingBid, numberOfDoors, numberOfSeats, loadCapacity);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count().Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Be(expectedError);
    }
}
