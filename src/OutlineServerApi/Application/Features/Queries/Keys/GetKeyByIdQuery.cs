using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Dtos.Responses.Keys;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using OutlineServerApi.Infrastructure.OutlineApi;
using SharedKernel.Results;
using System.Security.Claims;

namespace OutlineServerApi.Application.Features.Queries.Keys
{
    public sealed record GetKeyByIdQuery(Guid KeyId) : IQuery<KeyResponse>;

    internal class GetKeyByIdQueryHandler : IQueryHandler<GetKeyByIdQuery, KeyResponse>
    {
        private readonly OutlineContext _context;

        private readonly ErrorDescriber _errorDescriber;

        private readonly IAccessUriProvider _accessUriProvider;

        private readonly IHttpContextAccessor _contextAccessor;

        private readonly ILogger<GetKeyByIdQueryHandler> _logger;

        public GetKeyByIdQueryHandler(OutlineContext context,  ErrorDescriber errorDescriber, IAccessUriProvider accessUriProvider, ILogger<GetKeyByIdQueryHandler> logger, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _errorDescriber = errorDescriber;
            _accessUriProvider = accessUriProvider;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<KeyResponse>> Handle(GetKeyByIdQuery request, CancellationToken cancellationToken = default)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext!.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            string[] roles = _contextAccessor.HttpContext!.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToArray();
            Key? key = await _context.Keys
                .Include(k => k.Server)
                .ThenInclude(s => s.Country)
                .SingleOrDefaultAsync(k => k.Id == request.KeyId);

            if (key is null)
            {
                Result<KeyResponse> result = Result<KeyResponse>.FailedNotFound(_errorDescriber.KeyNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }

            if (key.CreatedByUser != userId && !roles.Any(r => r == "Admin"))
            {
                Result<KeyResponse> result = Result<KeyResponse>.FailedForbidden(_errorDescriber.KeyNotBelongToYou());
                _logger.LogWarning(result.ToString());
                return result;
            }

            return Result<KeyResponse>
                .Success(
                    new KeyResponse()
                    {
                        Id = key.Id,
                        Name = key.Name,
                        ServerAddress = key.Server!.Hostname,
                        CreatedByUser = key.CreatedByUser,
                        AccessUri = _accessUriProvider.GetAccessUri(key.Id).AbsoluteUri,
                        Country = key.Server.Country.Name
                    });
        }
    }
}
