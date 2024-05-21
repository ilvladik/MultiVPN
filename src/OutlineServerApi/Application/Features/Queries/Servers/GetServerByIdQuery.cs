using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Dtos.Responses.Servers;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using OutlineServerApi.Helpers;
using SharedKernel.Results;

namespace OutlineServerApi.Application.Features.Queries.Servers
{
    public record GetServerByIdQuery(Guid Id) : IQuery<ServerResponse>;

    internal class GetServerByIdQueryHandler : IQueryHandler<GetServerByIdQuery, ServerResponse>
    {
        private readonly OutlineContext _context;

        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<GetServerByIdQueryHandler> _logger;

        public GetServerByIdQueryHandler(
            OutlineContext context, 
            ErrorDescriber errorDescriber, 
            ILogger<GetServerByIdQueryHandler> logger)
        {
            _context = context;
            _errorDescriber = errorDescriber;
            _logger = logger;
        }

        public async Task<Result<ServerResponse>> Handle(GetServerByIdQuery request, CancellationToken cancellationToken)
        {
            Server? server = await _context.Servers
                .Include(s => s.Country)
                .Include(s => s.Keys)
                .SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (server is null)
            {
                Result<ServerResponse> result = Result<ServerResponse>.FailedNotFound(_errorDescriber.ServerNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }

            return Result<ServerResponse>.Success(
                new ServerResponse()
                {
                    Id = server.Id,
                    Name = server.Name,
                    ApiUrl = OutlineApiHelper.GetUri(server.Hostname, server.Port, server.ApiPrefix).AbsoluteUri.ToString(),
                    ServerAddress = server.Hostname,
                    IsAvailable = server.IsAvailable,
                    KeysCount = server.Keys.Count,
                    Country = string.Empty
                }) ;
        }
    }
}
