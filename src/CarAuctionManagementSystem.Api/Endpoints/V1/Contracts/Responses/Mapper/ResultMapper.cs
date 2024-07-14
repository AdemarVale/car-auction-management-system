using FluentResults;

namespace CarAuctionManagementSystem.Api.Endpoints.V1.Contracts.Responses.Mapper;

public static class ResultMapper
{
    private static readonly Func<IList<IError>, int, ErrorResponse> Map = (errorsResult, statusCode) =>
    {
        var errors = errorsResult.ToDictionary(x => x.Reasons[0].Message, y => new List<string> { y.Message });

        return new ErrorResponse(statusCode, errors);
    };

    public static ErrorResponse MapToErrorResponse(this IList<IError> errors, int statusCode)
    {
        return Map(errors, statusCode);
    }
}
