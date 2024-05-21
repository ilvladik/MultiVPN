using IdentityServerApi.Core.Models;
using IdentityServerApi.Infrastructure.Jwt.Models;
using IdentityServerApi.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityServerApi.Infrastructure.Jwt
{
    internal class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public Token GetAccessToken(User user, IEnumerable<string> roles)
        {

            Claim[] claims = [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                ..roles.Select(r => new Claim(ClaimTypes.Role, r))
            ];
            DateTime time = DateTime.UtcNow.Add(TimeSpan.FromDays(1));
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: time,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_options.Key)),
                    SecurityAlgorithms.HmacSha256));

            return new Token()
            {
                Value = new JwtSecurityTokenHandler().WriteToken(jwt),
                ExpiresIn = time
            };
        }

        public Token GetRefreshToken() =>
            new Token
            {
                Value = Guid.NewGuid().ToString(),
                ExpiresIn = DateTime.UtcNow.AddDays(1)
            };
    }
}
