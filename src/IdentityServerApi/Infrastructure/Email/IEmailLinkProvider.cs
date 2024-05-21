using IdentityServerApi.Core.Models;

namespace IdentityServerApi.Infrastructure.Email
{
    internal interface IEmailLinkProvider
    {
        Task<string> GenerateConfirmationLinkAsync(User user, Dictionary<string, string>? extraValues = default);

        Task<string> GeneratePasswordResetLinkAsync(User user);
    }
}
