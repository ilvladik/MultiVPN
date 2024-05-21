using IdentityServerApi.Application.Abstractions;
using IdentityServerApi.Application.Dtos.Responses;
using IdentityServerApi.Application.Errors;
using IdentityServerApi.Core;
using IdentityServerApi.Core.Models;
using IdentityServerApi.Infrastructure.Jwt;
using IdentityServerApi.Infrastructure.Jwt.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Results;

namespace IdentityServerApi.Application.Features.Commands
{
    public sealed record RefreshCommand(string ResfreshToken) : ICommand<AccessTokenResponse> { }

    internal sealed class RefreshCommandHandler : ICommandHandler<RefreshCommand, AccessTokenResponse>
    {
        private readonly IdentityContext _context;

        private readonly UserManager<User> _userManager;

        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<RefreshCommandHandler> _logger;

        private readonly IJwtProvider _jwtProvider;

        public RefreshCommandHandler(
            IdentityContext context, 
            UserManager<User> userManager,
            ErrorDescriber errorDescriber,
            IJwtProvider jwtProvider,
            ILogger<RefreshCommandHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _errorDescriber = errorDescriber;
            _jwtProvider = jwtProvider;
            _logger = logger;
        }

        public async Task<Result<AccessTokenResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken = _context.RefreshTokens.SingleOrDefault(r => r.Value == request.ResfreshToken);

            if (refreshToken is null || refreshToken.ExpiresIn <= DateTime.UtcNow)
            {
                var result = Result<AccessTokenResponse>.FailedNotFound(_errorDescriber.InvalidRefreshToken());
                _logger.LogWarning(result.ToString());
                return result;
            }

            Token token = _jwtProvider.GetRefreshToken();
            refreshToken.Value = token.Value;
            refreshToken.ExpiresIn = token.ExpiresIn;
            await _context.SaveChangesAsync();

            Token accessToken = _jwtProvider.GetAccessToken(refreshToken.User, await _userManager.GetRolesAsync(refreshToken.User));
            return Result<AccessTokenResponse>.Success(
                new AccessTokenResponse
                {
                    AccessToken = accessToken.Value,
                    ExpiresIn = accessToken.ExpiresIn.Ticks,
                    RefreshToken = token.Value
                });
        }
    }
}
