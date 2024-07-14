using FluentResults;
using FluentValidation.Results;

namespace CarAuctionManagementSystem.Application.Common.Mappers;
public static class ValidationResultMapper
{
    public static Result MapToFailedResult(this ValidationResult result)
    {
        return Result.Fail(result.Errors.Select(x => new Error(x.ErrorMessage).CausedBy(x.PropertyName)));
    }
}
