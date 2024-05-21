using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using SharedKernel.Results;

namespace OutlineServerApi.Application.Features.Commands.Countries
{
    public sealed record DeleteCountryCommand(Guid Id) : ICommand;

    internal class DeleteCountryCommandHandler : ICommandHandler<DeleteCountryCommand>
    {
        private readonly OutlineContext _context;

        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<DeleteCountryCommandHandler> _logger;

        public DeleteCountryCommandHandler(
            OutlineContext context, 
            ErrorDescriber errorDescriber, 
            ILogger<DeleteCountryCommandHandler> logger)
        {
            _context = context;
            _errorDescriber = errorDescriber;
            _logger = logger;
        }
        public async Task<Result> Handle(DeleteCountryCommand request, CancellationToken cancellationToken = default)
        {
            Country? country = await _context.Countries
                .SingleOrDefaultAsync(c => c.Id == request.Id);

            if (country is null)
            {
                Result result = Result.FailedNotFound(_errorDescriber.CountryNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return Result.Success;
        }
    }
}
