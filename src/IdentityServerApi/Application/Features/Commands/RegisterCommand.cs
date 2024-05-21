using IdentityServerApi.Application.Abstractions;
using IdentityServerApi.Core.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Results;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using IdentityServerApi.Application.Extensions;
using IdentityServerApi.Infrastructure.Email;

namespace IdentityServerApi.Application.Features.Commands
{
    public sealed record class RegisterCommand(string Email, string Password, string? CallbackUri) : ICommand { }

    internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
    {
        private readonly UserManager<User> _userManager;

        private readonly IEmailSender<User> _emailSender;

        private readonly IEmailLinkProvider _emailLinkProvider;

        public RegisterCommandHandler(
            UserManager<User> userManager, 
            IEmailSender<User> emailSender,
            IEmailLinkProvider emailConfirmationLinkProvider) 
        {  
            _userManager = userManager; 
            _emailSender = emailSender;
            _emailLinkProvider = emailConfirmationLinkProvider;
        }
        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email) || !new EmailAddressAttribute().IsValid(request.Email))
            {
                IdentityError error = _userManager.ErrorDescriber.InvalidEmail(request.Email);
                return Result.FailedValidation(new Error { Code = error.Code, Description = error.Description});
            }

            User user = new User { Email = request.Email, UserName = request.Email };

            IdentityResult identityResult = await _userManager.CreateAsync(user, request.Password);
            if (!identityResult.Succeeded)
                return identityResult.ToValidationResult();
            Dictionary<string, string> queryValues = new Dictionary<string, string>()
            {
                { "callbackUri", request.CallbackUri ?? ""}
            };

            await _userManager.AddToRoleAsync(user, "User");
            string confirmationLink = await _emailLinkProvider.GenerateConfirmationLinkAsync(user, queryValues);
            await _emailSender.SendConfirmationLinkAsync(user, request.Email, HtmlEncoder.Default.Encode(confirmationLink));
            return Result.Success;
        }
    }
}
