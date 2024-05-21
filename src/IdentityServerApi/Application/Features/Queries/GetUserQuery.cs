using IdentityServerApi.Application.Abstractions;
using IdentityServerApi.Application.Dtos.Responses;
using IdentityServerApi.Core.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Results;
using System.Security.Claims;

namespace IdentityServerApi.Application.Features.Queries
{
    public record class GetUserQuery() : IQuery<UserResponse> { }

    internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse>
    {
        private readonly UserManager<User> _userManager;

        private readonly IHttpContextAccessor _contextAccessor;

        private readonly ILogger<GetUserQueryHandler> _logger;

        public GetUserQueryHandler(UserManager<User> userManager, IHttpContextAccessor contextAccessor, ILogger<GetUserQueryHandler> logger)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            string userId = _contextAccessor.HttpContext!.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                var result = Result<UserResponse>.FailedForbidden();
                _logger.LogWarning(result.ToString());
                return result;
            }
            string[] roles = (await _userManager.GetRolesAsync(user)).ToArray();
            return Result<UserResponse>.Success(new UserResponse
                {
                    Id = user.Id.ToString(),
                    Email = user.Email!,
                    Roles = roles
                });
        }
    }
}
