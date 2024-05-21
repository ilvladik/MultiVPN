using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Dtos.Responses.Servers;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using OutlineServerApi.Helpers;
using SharedKernel.Results;

namespace OutlineServerApi.Application.Features.Queries.Servers
{
    public sealed record GetAllServersQuery : IQuery<IEnumerable<ServerResponse>>;

    internal class GetAllServersHandler : IQueryHandler<GetAllServersQuery, IEnumerable<ServerResponse>>
    {
        private readonly OutlineContext _context;

        public GetAllServersHandler(OutlineContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<ServerResponse>>> Handle(GetAllServersQuery request, CancellationToken cancellationToken)
        {
            return Result<IEnumerable<ServerResponse>>.Success(
                await _context.Servers
                    .Select(s => new ServerResponse()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        ApiUrl = OutlineApiHelper.GetUri(s.Hostname, s.Port, s.ApiPrefix).AbsoluteUri.ToString(),
                        ServerAddress = s.Hostname,
                        IsAvailable = s.IsAvailable,
                        KeysCount = s.Keys.Count,
                        Country = s.Country.Name
                    })
                    .ToArrayAsync());
        }
    }
}
