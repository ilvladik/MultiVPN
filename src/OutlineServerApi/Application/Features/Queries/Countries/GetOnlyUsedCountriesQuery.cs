using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Application.Dtos.Responses.Countries;
using OutlineServerApi.Core;
using SharedKernel.Results;

namespace OutlineServerApi.Application.Features.Queries.Countries
{
    public sealed record GetOnlyUsedCountriesQuery() : IQuery<IEnumerable<CountryResponse>> { }

    internal class GetOnlyUsedCountriesQueryHandler : IQueryHandler<GetOnlyUsedCountriesQuery, IEnumerable<CountryResponse>>
    {
        private readonly OutlineContext _context;

        public GetOnlyUsedCountriesQueryHandler(OutlineContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<CountryResponse>>> Handle(GetOnlyUsedCountriesQuery request, CancellationToken cancellationToken = default)
        {
            return Result<IEnumerable<CountryResponse>>
                .Success( 
                    await _context.Countries
                        .Where(c => _context.Servers.Any(s => s.CountryId == c.Id))
                        .Select(c => new CountryResponse()
                        { 
                            Id = c.Id,
                            Name = c.Name
                        })
                        .ToArrayAsync());
        }
    }
}
