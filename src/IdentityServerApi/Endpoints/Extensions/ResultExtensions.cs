using SharedKernel.Results;

namespace IdentityServerApi.Endpoints.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToApiResult(this Result result) =>
            result.Type switch
            {
                ResultType.Ok => Results.Ok(result),
                ResultType.NotFound => Results.NotFound(result),
                ResultType.Validation => Results.BadRequest(result),
                ResultType.Conflict => Results.Conflict(result),
                ResultType.Forbidden => Results.Forbid(),
                ResultType.Unauthorized => Results.Unauthorized(),
                _ => Results.StatusCode(StatusCodes.Status500InternalServerError)
            };
    }
}
