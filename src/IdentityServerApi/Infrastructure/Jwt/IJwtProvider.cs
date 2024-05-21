using IdentityServerApi.Core.Models;
using IdentityServerApi.Infrastructure.Jwt.Models;

namespace IdentityServerApi.Infrastructure.Jwt
{
    internal interface IJwtProvider
    {
        Token GetAccessToken(User user, IEnumerable<string> roles);

        Token GetRefreshToken();
    }
}
