using IdentityServerApi.Application.Abstractions;
using IdentityServerApi.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SharedKernel.Results;
using System.Text;

namespace IdentityServerApi.Application.Features.Commands
{
    public record class ResetPasswordCommand(string Email, string ResetCode, string NewPassword) : ICommand { }

    internal sealed class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
    {
        private readonly UserManager<User> _userManager;

        private readonly ILogger<ResetPasswordCommandHandler> _logger;

        public ResetPasswordCommandHandler(UserManager<User> userManager, ILogger<ResetPasswordCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            IdentityResult identityResult;
            if (user is null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                identityResult = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
                var result = Result.FailedForbidden(identityResult.Errors
                    .Select(e => new Error()
                    {
                        Code = e.Code,
                        Description = e.Description
                    }).ToArray());
                _logger.LogWarning(result.ToString());
                return result;
            }

            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetCode));
                identityResult = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
            }
            catch (FormatException)
            {
                identityResult = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
                var result = Result.FailedForbidden(identityResult.Errors
                    .Select(e => new Error()
                    {
                        Code = e.Code,
                        Description = e.Description
                    }).ToArray());
                _logger.LogWarning(result.ToString());
                return result;
            }

            if (!identityResult.Succeeded)
            {
                var result = Result.FailedValidation(identityResult.Errors
                    .Select(e => new Error()
                    {
                        Code = e.Code,
                        Description = e.Description
                    }).ToArray());
                _logger.LogWarning(result.ToString());
                return result;
            }

            return Result.Success;
        }
    }
}
