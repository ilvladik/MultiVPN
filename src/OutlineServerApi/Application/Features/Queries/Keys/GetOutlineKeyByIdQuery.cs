using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Dtos.Responses.Keys;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using SharedKernel.Results;

namespace OutlineServerApi.Application.Features.Queries.Keys
{
    public sealed record GetOutlineKeyByIdQuery(Guid KeyId) : IQuery<OutlineKeyForAppResponse>;

    internal class GetOutlineKeyByIdQueryHandler : IQueryHandler<GetOutlineKeyByIdQuery, OutlineKeyForAppResponse>
    {
        private readonly OutlineContext _context;

        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<GetOutlineKeyByIdQueryHandler> _logger;

        public GetOutlineKeyByIdQueryHandler(OutlineContext context, ErrorDescriber errorDescriber, ILogger<GetOutlineKeyByIdQueryHandler> logger)
        {
            _context = context;
            _errorDescriber = errorDescriber;
            _logger = logger;
        }

        public async Task<Result<OutlineKeyForAppResponse>> Handle(GetOutlineKeyByIdQuery request, CancellationToken cancellationToken = default)
        {
            Key? key = await _context.Keys
                .Include(k => k.Server)
                .SingleOrDefaultAsync(k => k.Id == request.KeyId, cancellationToken);
            if (key is null)
            {
                Result<OutlineKeyForAppResponse> result = Result<OutlineKeyForAppResponse>.FailedNotFound(_errorDescriber.KeyNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }

            return Result<OutlineKeyForAppResponse>.Success(
                new OutlineKeyForAppResponse()
                {
                    Server = key.Server.Hostname,
                    Port = key.Port.ToString(),
                    Password = key.Password,
                    Method = key.Method
                });
        }
    }
}
