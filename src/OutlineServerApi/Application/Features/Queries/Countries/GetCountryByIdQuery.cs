using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Dtos.Responses.Countries;
using OutlineServerApi.Application.Errors;
using OutlineServerApi.Core;
using OutlineServerApi.Core.Models;
using SharedKernel.Results;

namespace OutlineServerApi.Application.Features.Queries.Countries
{
    public sealed record GetCountryByIdQuery(Guid CountryId) : IQuery<CountryResponse> { }

    internal class GetCountryByIdQueryHandler : IQueryHandler<GetCountryByIdQuery, CountryResponse>
    {
        private readonly OutlineContext _context;

        private readonly ErrorDescriber _errorDescriber;

        private readonly ILogger<GetCountryByIdQueryHandler> _logger;

        public GetCountryByIdQueryHandler(OutlineContext context, ErrorDescriber errorDescriber, ILogger<GetCountryByIdQueryHandler> logger)
        {
            _context = context;
            _errorDescriber = errorDescriber;
            _logger = logger;
        }

        public async Task<Result<CountryResponse>> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken = default)
        {
            Country? country = await _context.Countries
                .SingleOrDefaultAsync(c => c.Id == request.CountryId);

            if (country is null)
            {
                Result<CountryResponse> result = Result<CountryResponse>.FailedNotFound(_errorDescriber.CountryNotFound());
                _logger.LogWarning(result.ToString());
                return result;
            }

            return Result<CountryResponse>.Success(
                new CountryResponse()
                { 
                    Id = country.Id,
                    Name = country.Name
                });
        }
    }
}
