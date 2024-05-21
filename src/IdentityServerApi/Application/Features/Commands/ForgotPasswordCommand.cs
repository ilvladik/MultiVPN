using IdentityServerApi.Application.Abstractions;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Results;
using System.Text.Encodings.Web;
using IdentityServerApi.Core.Models;
using IdentityServerApi.Infrastructure.Email;

namespace IdentityServerApi.Application.Features.Commands
{
    public sealed record ForgotPasswordCommand(string Email) : ICommand { }

    internal sealed class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
    {
        private readonly UserManager<User> _userManager;

        private readonly IEmailLinkProvider _emailLinkProvider;

        private readonly IEmailSender<User> _emailSender;

        public ForgotPasswordCommandHandler(UserManager<User> userManager, IEmailSender<User> emailSender, IEmailLinkProvider emailLinkProvider) 
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _emailLinkProvider = emailLinkProvider;
        }
        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is not null && await _userManager.IsEmailConfirmedAsync(user))
            {
                string resetPasswordLink = await _emailLinkProvider.GeneratePasswordResetLinkAsync(user);

                await _emailSender.SendPasswordResetLinkAsync(user, request.Email, HtmlEncoder.Default.Encode(resetPasswordLink));
            }

            return Result.Success;
        }
    }
}
