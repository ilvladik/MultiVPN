using Microsoft.AspNetCore.Identity;
using SharedKernel.Results;

namespace IdentityServerApi.Application.Extensions
{
    internal static class IdentityResultExtensions
    {
        public static Result ToNotFoundResult(this IdentityResult identityResult) =>
            Result.FailedNotFound(identityResult.GetErrors());

        public static Result ToValidationResult(this IdentityResult identityResult) =>
            Result.FailedValidation(identityResult.GetErrors());

        public static Result ToConflictResult(this IdentityResult identityResult) =>
            Result.FailedConflict(identityResult.GetErrors());

        public static Result ToForbiddenResult(this IdentityResult identityResult) =>
            Result.FailedForbidden(identityResult.GetErrors());

        public static Result ToUnauthorizedResult(this IdentityResult identityResult) =>
            Result.FailedUnauthorized(identityResult.GetErrors());

        private static Error[] GetErrors(this IdentityResult identityResult) 
        {
            return identityResult.Errors
                    .Select(e => new Error()
                    {
                        Code = e.Code,
                        Description = e.Description
                    }).ToArray();
        }
    }
}
