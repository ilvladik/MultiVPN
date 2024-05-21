using IdentityServerApi.Application.Abstractions;
using IdentityServerApi.Application.Errors;
using IdentityServerApi.Application.Extensions;
using IdentityServerApi.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SharedKernel.Results;
using System.Text;

namespace IdentityServerApi.Application.Features.Commands
{
    public sealed record ConfirmEmailCommand(string UserId, string Code) : ICommand { }

    internal sealed class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand>
    {
        private readonly UserManager<User> _userManager;

        public ConfirmEmailCommandHandler(UserManager<User> userManager)
        {  
            _userManager = userManager;
        }

        public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
                return Result.FailedUnauthorized();

            string code;
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            }
            catch (FormatException)
            {
                return Result.FailedUnauthorized();
            }

            IdentityResult identityResult = await _userManager.ConfirmEmailAsync(user, code);
            if (!identityResult.Succeeded)
                return Result.FailedUnauthorized();

            return Result.Success;
        }
    }
}
