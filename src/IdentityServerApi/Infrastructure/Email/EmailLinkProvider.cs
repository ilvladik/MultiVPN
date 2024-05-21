using IdentityServerApi.Core.Models;
using IdentityServerApi.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text;

namespace IdentityServerApi.Infrastructure.Email
{
    internal class EmailLinkProvider : IEmailLinkProvider
    {
        private readonly UserManager<User> _userManager;

        private readonly LinkGenerator _linkGenerator;

        private readonly IHttpContextAccessor _contextAccessor;

        private readonly ExternalLinksOptions _externalLinkOptions;

        public EmailLinkProvider(
            UserManager<User> userManager, 
            LinkGenerator linkGenerator, 
            IOptions<ExternalLinksOptions> externalLinkOptions,
            IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _linkGenerator = linkGenerator;
            _externalLinkOptions = externalLinkOptions.Value;
            _contextAccessor = contextAccessor;
        }

        public async Task<string> GenerateConfirmationLinkAsync(User user, Dictionary<string, string>? extraValues = default)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var userId = await _userManager.GetUserIdAsync(user);
            var routeValues = new Dictionary<string, string>()
            {
                ["userId"] = userId,
                ["code"] = code,
            }.Union(extraValues);

            string confirmEmailEndpointName = "confirmEmail";
            string link = _linkGenerator.GetUriByName(_contextAccessor.HttpContext!, confirmEmailEndpointName)
                ?? throw new NotSupportedException($"Could not find endpoint named '{confirmEmailEndpointName}'.");
            return link + QueryString.Create(routeValues!);
        }

        public async Task<string> GeneratePasswordResetLinkAsync(User user)
        {
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            string? email = await _userManager.GetEmailAsync(user);
            Dictionary<string, string> routeValues = new()
            {
                ["email"] = email ?? string.Empty,
                ["code"] = code,
            };

            string clientLink = _externalLinkOptions.ResetPasswordLink;
            UriBuilder uriBuilder = new(clientLink)
            {
                Query = QueryString.Create(routeValues!).ToUriComponent()
            };
            return uriBuilder.Uri.ToString();
        }
    }
}
