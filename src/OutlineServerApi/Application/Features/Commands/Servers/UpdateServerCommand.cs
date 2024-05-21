using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using SharedKernel.Results;

namespace OutlineServerApi.Application.Features.Commands.Servers
{
    public sealed record UpdateServerCommand(Guid ServerId, string Name, bool IsAvailable) : ICommand { };

    internal class UpdateServerCommandHandler : ICommandHandler<UpdateServerCommand>
    {
        private readonly OutlineContext _context;

        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<UpdateServerCommandHandler> _logger;

        public UpdateServerCommandHandler(
            OutlineContext context, 
            ErrorDescriber errorDescriber, 
            ILogger<UpdateServerCommandHandler> logger)
        {   
            _context = context;
            _errorDescriber = errorDescriber;
            _logger = logger;
        }

        public async Task<Result> Handle(UpdateServerCommand request, CancellationToken cancellationToken = default)
        {
            Server? server = await _context.Servers
                .SingleOrDefaultAsync(s => s.Id == request.ServerId);

            if (server is null)
            {
                Result result = Result.FailedNotFound(_errorDescriber.ServerNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }

            server.Name = request.Name;
            server.IsAvailable = request.IsAvailable;
            await _context.SaveChangesAsync();

            return Result.Success;
        }
    }
}
