using SharedKernel.Results;
using Microsoft.EntityFrameworkCore;
using OutlineServerApi.Application.Abstractions;
using OutlineServerApi.Core;
using OutlineServerApi.Application.Dtos.Responses.Countries;

namespace OutlineServerApi.Application.Features.Queries.Countries
{
    public sealed record GetAllCountriesQuery() : IQuery<IEnumerable<CountryResponse>> { }

    internal class GetAllCountriesQueryHandler : IQueryHandler<GetAllCountriesQuery, IEnumerable<CountryResponse>>
    {
        private readonly OutlineContext _context;

        public GetAllCountriesQueryHandler(OutlineContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<CountryResponse>>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken = default)
        {
            return Result<IEnumerable<CountryResponse>>
                .Success(
                    await _context.Countries
                        .Select(c => new CountryResponse()
                        {
                            Id = c.Id,
                            Name = c.Name
                        })
                        .ToArrayAsync());
        }
    }
}
