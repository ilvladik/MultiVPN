using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Dtos.Responses.Keys;
using OutlineServerApi.Core;
using OutlineServerApi.Infrastructure.OutlineApi;
using SharedKernel.Results;
using System.Security.Claims;

namespace OutlineServerApi.Application.Features.Queries.Keys
{
    public sealed record GetAllKeysQuery : IQuery<IEnumerable<KeyResponse>>;

    internal class GetAllKeysQueryHandler : IQueryHandler<GetAllKeysQuery, IEnumerable<KeyResponse>>
    {
        private readonly OutlineContext _context;

        private readonly IAccessUriProvider _accessUriProvider;

        private readonly IHttpContextAccessor _contextAccessor;

        public GetAllKeysQueryHandler(OutlineContext context, IAccessUriProvider accessUriProvider, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _accessUriProvider = accessUriProvider;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result<IEnumerable<KeyResponse>>> Handle(GetAllKeysQuery request, CancellationToken cancellationToken = default)
        {
            string[] roles = _contextAccessor.HttpContext!.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
            if (roles.Any(r => r == "Admin"))
            {
                return Result<IEnumerable<KeyResponse>>.Success(
                await _context.Keys
                    .Select(k => new KeyResponse()
                    {
                        Id = k.Id,
                        Name = k.Name,
                        ServerAddress = k.Server!.Hostname,
                        CreatedByUser = k.CreatedByUser,
                        AccessUri = _accessUriProvider.GetAccessUri(k.Id).AbsoluteUri,
                        Country = k.Server.Country.Name
                    })
                    .ToArrayAsync());
            }
            else
            {
                Guid userId = Guid.Parse(_contextAccessor.HttpContext!.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
                return Result<IEnumerable<KeyResponse>>.Success(
                await _context.Keys.Where(k => k.CreatedByUser == userId)
                    .Select(k => new KeyResponse()
                    {
                        Id = k.Id,
                        Name = k.Name,
                        ServerAddress = k.Server!.Hostname,
                        CreatedByUser = k.CreatedByUser,
                        AccessUri = _accessUriProvider.GetAccessUri(k.Id).AbsoluteUri,
                        Country = k.Server.Country.Name
                    })
                    .ToArrayAsync());
            }
        }
    }
}
