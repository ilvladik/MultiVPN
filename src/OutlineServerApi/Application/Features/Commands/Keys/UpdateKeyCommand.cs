using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using SharedKernel.Results;
using System.Data;
using System.Security.Claims;

namespace OutlineServerApi.Application.Features.Commands.Keys
{
    public sealed record UpdateKeyCommand(Guid KeyId, string Name) : ICommand {}

    internal class UpdateKeyCommandHandler : ICommandHandler<UpdateKeyCommand>
    {
        private readonly OutlineContext _context;

        private readonly ErrorDescriber _errorDescriber;

        private readonly IHttpContextAccessor _contextAccessor;

        private readonly ILogger<UpdateKeyCommandHandler> _logger;

        public UpdateKeyCommandHandler(
            OutlineContext context, 
            ErrorDescriber errorDescriber, 
            IHttpContextAccessor contextAccessor,
            ILogger<UpdateKeyCommandHandler> logger) 
        {
            _context = context;
            _errorDescriber = errorDescriber;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        public async Task<Result> Handle(UpdateKeyCommand request, CancellationToken cancellationToken = default)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext!.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            string[] roles = _contextAccessor.HttpContext!.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
            Key? key = await _context.Keys
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

            key.Name = request.Name;
            await _context.SaveChangesAsync();
            return Result.Success;
        }
    }
}
