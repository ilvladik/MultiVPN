using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using OutlineServerApi.Helpers;
using OutlineServerApi.Infrastructure.OutlineApi;
using OutlineServerApi.Infrastructure.OutlineApi.Models;
using SharedKernel.Results;

namespace OutlineServerApi.Application.Features.Commands.Servers
{
    public sealed record DeleteServerCommand(Guid ServerId) : ICommand;

    internal class DeleteServerCommandHandler : ICommandHandler<DeleteServerCommand>
    {
        private readonly OutlineContext _context;

        private readonly IOutlineApi _outlineApi;
        
        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<DeleteServerCommandHandler> _logger;

        public DeleteServerCommandHandler(
            OutlineContext context, 
            IOutlineApi outlineApi, 
            ErrorDescriber errorDescriber, 
            ILogger<DeleteServerCommandHandler> logger)
        {
            _context = context;
            _outlineApi = outlineApi;
            _errorDescriber = errorDescriber;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteServerCommand request, CancellationToken cancellationToken = default)
        {
            Server? source = await _context.Servers
                .SingleOrDefaultAsync(s => s.Id == request.ServerId);

            if (source is null)
            {
                Result result = Result.FailedNotFound(_errorDescriber.ServerNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }

            Server? dest = _context.Servers
                .Where(s => s.Id != request.ServerId && s.CountryId == source.CountryId && s.IsAvailable)
                .MinBy(s => s.Keys.Count);

            if (dest is null)
            {
                Result result = Result.FailedNotFound(_errorDescriber.AllServersNotAvailable());
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
            _context.Servers.Remove(source);
            await _context.SaveChangesAsync();

            return Result.Success;
        }
    }
}
