using MediatR;
using Microsoft.AspNetCore.Mvc;
using OutlineServerApi.Application.Dtos.Requests.Countries;
using OutlineServerApi.Application.Dtos.Responses.Countries;
using OutlineServerApi.Application.Features.Commands.Countries;
using OutlineServerApi.Application.Features.Queries.Countries;
using OutlineServerApi.Endpoints.Extensions;
using SharedKernel.Results;

namespace OutlineServerApi.Endpoints
{
    public static class CountriesEndpoints
    {
        public static void MapCounriesEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/countries");

            group.MapPost(string.Empty,
                async ([FromBody] CreateNewCountryRequest request, ISender sender) =>
                {

                    Result<CountryResponse> result = await sender.Send(new CreateNewCountryCommand(request.Name));
                    return result.ToApiResult();
                })
                .RequireAuthorization("AdminPolicy");

            group.MapDelete("{countryId}",
                async (Guid countryId, ISender sender) =>
                {
                    Result result = await sender.Send(new DeleteCountryCommand(countryId));
                    return result.ToApiResult();
                })
                .RequireAuthorization("AdminPolicy");

            group.MapGet(string.Empty,
                async ([FromQuery] bool? onlyUsed,ISender sender) =>
                {
                    Result<IEnumerable<CountryResponse>> result;
                    if (onlyUsed is null || !onlyUsed.Value)
                        result = await sender.Send(new GetAllCountriesQuery());
                    else
                        result = await sender.Send(new GetOnlyUsedCountriesQuery());
                    return result.ToApiResult();
                });
            group.MapGet("/{countryId}",
                async (Guid countryId, ISender sender) =>
                {
                    Result<CountryResponse> result = result = await sender.Send(new GetCountryByIdQuery(countryId));
                    return result.ToApiResult();
                });
        }
    }
}
