using IdentityServerApi.Application.Abstractions;
using IdentityServerApi.Core.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Results;
using System.Text.Encodings.Web;
using IdentityServerApi.Infrastructure.Email;

namespace IdentityServerApi.Application.Features.Commands
{
    public sealed record ResendConfirmationEmailCommand(string Email) : ICommand { }

    internal sealed class ResendConfirmationEmailCommandHandler : ICommandHandler<ResendConfirmationEmailCommand>
    {
        private readonly UserManager<User> _userManager;

        private readonly IEmailSender<User> _emailSender;

        private readonly IEmailLinkProvider _emailConfirmationLinkProvider;

        public ResendConfirmationEmailCommandHandler(
            UserManager<User> userManager,
            IEmailSender<User> emailSender,
            IEmailLinkProvider emailConfirmationLinkProvider)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _emailConfirmationLinkProvider = emailConfirmationLinkProvider;
        }
        public async Task<Result> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            {
                return Result.Success;
            }

            string confirmationLink = await _emailConfirmationLinkProvider.GenerateConfirmationLinkAsync(user);
            await _emailSender.SendConfirmationLinkAsync(user, request.Email, HtmlEncoder.Default.Encode(confirmationLink));
            return Result.Success;
        }
    }
}
