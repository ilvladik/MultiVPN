using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using OutlineServerApi.Helpers;
using OutlineServerApi.Infrastructure.OutlineApi;
using OutlineServerApi.Infrastructure.OutlineApi.Models;
using SharedKernel.Results;

namespace OutlineServerApi.Application.Features.Commands.Keys
{
    public sealed record TransferKeysCommand(Guid SourceId, Guid DestinationId) : ICommand;

    internal class TransferKeysCommandHandler : ICommandHandler<TransferKeysCommand>
    {
        private readonly OutlineContext _context;

        private readonly IOutlineApi _outlineApi;

        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<TransferKeysCommandHandler> _logger;

        public TransferKeysCommandHandler(
            OutlineContext context,
            IOutlineApi outlineApi,
            ErrorDescriber errorDescriber,
            ILogger<TransferKeysCommandHandler> logger)
        {
            _context = context;
            _outlineApi = outlineApi;
            _errorDescriber = errorDescriber;
            _logger = logger;
        }

        public async Task<Result> Handle(TransferKeysCommand request, CancellationToken cancellationToken = default)
        {
            Server? source = await _context.Servers
                .SingleOrDefaultAsync(s => s.Id == request.SourceId);

            Server? dest = await _context.Servers
                .SingleOrDefaultAsync(s => s.Id == request.DestinationId);

            if (source is null)
            {
                Result result = Result.FailedNotFound(_errorDescriber.ServerNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }

            if (dest is null)
            {
                Result result = Result.FailedNotFound(_errorDescriber.ServerNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }

            if (!dest.IsAvailable)
            {
                Result result = Result.FailedNotFound(_errorDescriber.ServerNotAvailable(dest.Hostname));
                _logger.LogWarning(result.ToString());
                return result;
            }

            if (source.CountryId != dest.CountryId)
            {
                Result result = Result.FailedValidation(_errorDescriber.ServersHaveNotSameCountries(source.Hostname, dest.Hostname));
                _logger.LogWarning(result.ToString());
                return result;
            }

            foreach (Key key in source.Keys)
            {
                OutlineKey outlineKey = await _outlineApi.TransferKeyToNewServer(
                    OutlineApiHelper.GetUri(source.Hostname, source.Port, source.ApiPrefix),
                    OutlineApiHelper.GetUri(dest.Hostname, dest.Port, dest.ApiPrefix),
                    key.InternalId);
                key.ServerId = dest.Id;
                key.Name = outlineKey.Name;
                key.InternalId = outlineKey.Id;
                key.Password = outlineKey.Password;
                key.Port = outlineKey.Port;
                key.Method = outlineKey.Method;
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();

            return Result.Success;
        }
    }
}
