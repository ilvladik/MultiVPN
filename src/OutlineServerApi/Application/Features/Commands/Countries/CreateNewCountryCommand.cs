using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Dtos.Responses.Countries;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using SharedKernel.Results;

namespace OutlineServerApi.Application.Features.Commands.Countries
{
    public sealed record CreateNewCountryCommand(string Name) : ICommand<CountryResponse> { }

    internal class CreateNewCountryCommandHandler : ICommandHandler<CreateNewCountryCommand, CountryResponse>
    {
        private readonly OutlineContext _context;

        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<CreateNewCountryCommandHandler> _logger;

        public CreateNewCountryCommandHandler(
            OutlineContext context, 
            ErrorDescriber errorDescriber, 
            ILogger<CreateNewCountryCommandHandler> logger)
        {
            _context = context;
            _errorDescriber = errorDescriber;
            _logger = logger;
        }

        public async Task<Result<CountryResponse>> Handle(CreateNewCountryCommand request, CancellationToken cancellationToken = default)
        {
            if (_context.Countries.Any(c => c.Name == request.Name))
            {
                Result<CountryResponse> result = Result<CountryResponse>.FailedConflict(_errorDescriber.CountryWithThisNameAlreadyExists(request.Name));
                _logger.LogWarning(result.ToString());
                return result;
            }

            Country country = new Country
            { 
                Name = request.Name
            };
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return Result<CountryResponse>.Success(
                new CountryResponse()
                {
                    Id = country.Id,
                    Name = country.Name
                });
        }
    }
}
