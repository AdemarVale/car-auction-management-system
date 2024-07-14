using CarAuctionManagementSystem.Application.Features.Bids.Commands;
using CarAuctionManagementSystem.Application.Features.Bids.Commands.Validators;
using FluentAssertions;
using Xunit;

namespace CarAuctionManagementSystem.UnitTests.Tests.Application.Features.Bids.Commands.Validators;

public class CreateBidCommandValidatorTests
{
    private readonly CreateBidCommandValidator _validator;

    public CreateBidCommandValidatorTests()
    {
        _validator = new();
    }

    [Theory]
    [InlineData("id1", 50, "", "Invalid user identifier.")]
    [InlineData("id1", 0, "user", "Invalid value.")]
    [InlineData("", 50, "user", "Invalid vehicle identifier.")]
    public void Validate_ErrorFound_ShouldMatchExpectedError(
        string vehicleIdentifier,
        double value,
        string userIdentifier,
        string expectedError)
    {
        // Arrange
        var command = new CreateBidCommand(vehicleIdentifier, value, userIdentifier);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count().Should().Be(1);
        result.Errors[0].ErrorMessage.Should().Be(expectedError);
    }
}
