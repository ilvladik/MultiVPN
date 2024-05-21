using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using OutlineServerApi.Helpers;
using OutlineServerApi.Infrastructure.OutlineApi;
using SharedKernel.Results;
using System.Security.Claims;

namespace OutlineServerApi.Application.Features.Commands.Keys
{
    public sealed record DeleteKeyCommand(Guid KeyId) : ICommand;

    internal class DeleteKeyCommandHandler : ICommandHandler<DeleteKeyCommand>
    {
        private readonly OutlineContext _context;

        private readonly IOutlineApi _outlineApi;

        private readonly IHttpContextAccessor _contextAccessor;

        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<DeleteKeyCommandHandler> _logger;

        public DeleteKeyCommandHandler(
            OutlineContext context, 
            IOutlineApi outlineApi,
            IHttpContextAccessor contextAccessor,
            ErrorDescriber errorDescriber, 
            ILogger<DeleteKeyCommandHandler> logger)
        {
            _context = context;
            _outlineApi = outlineApi;
            _contextAccessor = contextAccessor;
            _logger = logger;
            _errorDescriber = errorDescriber;
        }

        public async Task<Result> Handle(DeleteKeyCommand request, CancellationToken cancellationToken = default)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext!.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            string[] roles = _contextAccessor.HttpContext!.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
            Key? key = await _context.Keys
                .Include(k => k.Server)
                .SingleOrDefaultAsync(k => k.Id == request.KeyId);

            if (key is null)
            {
                Result result = Result.FailedNotFound(_errorDescriber.KeyNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }
            if (key.CreatedByUser != userId && !roles.Any(r => r == "Admin"))
            {
                Result result = Result.FailedForbidden(_errorDescriber.KeyNotBelongToYou());
                _logger.LogWarning(result.ToString());
                return result;
            }

            _context.Keys.Remove(key);
            await _outlineApi
                .DeleteKeyAsync(
                    OutlineApiHelper.GetUri(key.Server.Hostname, key.Server.Port, key.Server.ApiPrefix), 
                    key.InternalId);

            await _context.SaveChangesAsync();
            return Result.Success;
        }
    }
}
