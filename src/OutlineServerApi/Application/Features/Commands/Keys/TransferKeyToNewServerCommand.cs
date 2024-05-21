using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using OutlineServerApi.Helpers;
using OutlineServerApi.Infrastructure.OutlineApi;
using SharedKernel.Results;

namespace OutlineServerApi.Application.Features.Commands.Keys
{
    public sealed record TransferKeyToNewServerCommand(Guid KeyId, Guid ServerId) : ICommand;

    internal class TransferKeyToNewServerCommandHandler : ICommandHandler<TransferKeyToNewServerCommand>
    {
        private readonly OutlineContext _context;

        private readonly IOutlineApi _outlineApi;

        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<TransferKeyToNewServerCommandHandler> _logger;

        public TransferKeyToNewServerCommandHandler(
            OutlineContext context, 
            IOutlineApi outlineApi, 
            ErrorDescriber errorDescriber, 
            ILogger<TransferKeyToNewServerCommandHandler> logger)
        {
            _context = context;
            _outlineApi = outlineApi;
            _errorDescriber = errorDescriber;
            _logger = logger;
        }

        public async Task<Result> Handle(TransferKeyToNewServerCommand request, CancellationToken cancellationToken = default)
        {
            Key? key = await _context.Keys
                .Include(k => k.Server)
                .SingleOrDefaultAsync(k => k.Id == request.KeyId);

            if (key is null)
            {
                Result result = Result.FailedNotFound(_errorDescriber.KeyNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }    
            if (key.ServerId == request.ServerId)
                return Result.Success;

            Server? dest = await _context.Servers
                .SingleOrDefaultAsync(s => s.Id == request.ServerId);
            
            if (dest is null)
            {
                Result result = Result.FailedNotFound(_errorDescriber.ServerNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }
            if (key.Server.CountryId != dest.CountryId)
            {
                Result result = Result.FailedValidation(_errorDescriber.ServersHaveNotSameCountries(key.Server.Hostname, dest.Hostname));
                _logger.LogWarning(result.ToString());
                return result;
            }

            var outlineKey = await _outlineApi
                .TransferKeyToNewServer(
                    OutlineApiHelper.GetUri(key.Server.Hostname, key.Server.Port, key.Server.ApiPrefix),
                    OutlineApiHelper.GetUri(dest.Hostname, dest.Port, dest.ApiPrefix),
                    key.InternalId);

            key.ServerId = dest.Id;
            key.Name = outlineKey.Name;
            key.InternalId = outlineKey.Id;
            key.Password = outlineKey.Password;
            key.Port = outlineKey.Port;
            key.Method = outlineKey.Method;

            await _context.SaveChangesAsync();
            return Result.Success;
        }
    }
}
