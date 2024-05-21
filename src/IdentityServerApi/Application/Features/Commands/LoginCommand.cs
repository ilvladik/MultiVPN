
using IdentityServerApi.Application.Abstractions;
using IdentityServerApi.Application.Dtos.Responses;
using IdentityServerApi.Application.Errors;
using IdentityServerApi.Core;
using IdentityServerApi.Core.Models;
using IdentityServerApi.Infrastructure.Jwt;
using IdentityServerApi.Infrastructure.Jwt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;

namespace IdentityServerApi.Application.Features.Commands
{
    public sealed record class LoginCommand(string Email, string Password) : ICommand<AccessTokenResponse> { }

    internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, AccessTokenResponse>
    {
        private readonly IdentityContext _context;

        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<LoginCommandHandler> _logger;

        private readonly IJwtProvider _jwtProvider;

        public LoginCommandHandler(
            IdentityContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ErrorDescriber errorDescriber,
            ILogger<LoginCommandHandler> logger,
            IJwtProvider jwtProvider) 
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _errorDescriber = errorDescriber;
            _jwtProvider = jwtProvider;
            _logger = logger;
        }
        public async Task<Result<AccessTokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) 
            {
                var result = Result<AccessTokenResponse>.FailedValidation(_errorDescriber.EmailNotFound(request.Email));
                _logger.LogWarning(result.ToString());
                return result;
            }

            var attempt = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (attempt.IsLockedOut)
            {
                var result = Result<AccessTokenResponse>.FailedValidation(_errorDescriber.SignInIsLockedOut());
                _logger.LogWarning(result.ToString());
                return result;
            }
            if (attempt.IsNotAllowed)
            {
                var result = Result<AccessTokenResponse>.FailedValidation(_errorDescriber.EmailNotConfirmed());
                _logger.LogWarning(result.ToString());
                return result;
            }
            if (!attempt.Succeeded)
            {
                var result = Result<AccessTokenResponse>.FailedValidation(_errorDescriber.InvalidEmailOrPassword());
                _logger.LogWarning(result.ToString());
                return result;
            }
            
            Token refreshToken = _jwtProvider.GetRefreshToken();
            RefreshToken? refreshDbToken = await _context.RefreshTokens
                .SingleOrDefaultAsync(c => c.UserId == user.Id);
            if (refreshDbToken is not null)
            {
                refreshDbToken.Value = refreshToken.Value.ToString();
                refreshDbToken.ExpiresIn = refreshToken.ExpiresIn;
                _context.RefreshTokens.Update(refreshDbToken);
            }
            else
            {
                refreshDbToken = new RefreshToken(refreshToken.Value, refreshToken.ExpiresIn, user);
                await _context.RefreshTokens.AddAsync(refreshDbToken);
            }
            await _context.SaveChangesAsync();
            
            Token accessToken =_jwtProvider.GetAccessToken(user, await _userManager.GetRolesAsync(user));
            return Result<AccessTokenResponse>.Success(
                new AccessTokenResponse
                {
                    AccessToken = accessToken.Value,
                    ExpiresIn = accessToken.ExpiresIn.Ticks,
                    RefreshToken = refreshToken.Value
                });
        }
    }
}
