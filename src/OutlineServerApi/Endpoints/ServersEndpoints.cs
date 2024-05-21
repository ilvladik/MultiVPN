using MediatR;
using Microsoft.AspNetCore.Mvc;
using OutlineServerApi.Application.Dtos.Requests.Servers;
using OutlineServerApi.Application.Dtos.Responses.Servers;
using OutlineServerApi.Application.Features.Commands.Servers;
using OutlineServerApi.Application.Features.Queries.Servers;
using OutlineServerApi.Endpoints.Extensions;
using SharedKernel.Results;
using System.Security.Claims;

namespace OutlineServerApi.Endpoints
{
    internal static class ServersEndpoints
    {
        public static void MapServersEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/servers")
                .RequireAuthorization("AdminPolicy");

            group.MapPost(string.Empty,
                async ([FromBody] AddExistingServerByApiUrlRequest request, ISender sender) =>
            {
                Result<ServerResponse> result = await sender.Send(new AddExistingServerByApiUrlCommand(request.Name, request.ApiUrl, request.CountryId ?? Guid.Empty));
                return result.ToApiResult();
            });

            group.MapPut("/{serverId}",
                async ([FromRoute] Guid serverId, [FromBody] UpdateServerRequest request, ISender sender) =>
            {
                Result result = await sender.Send(new UpdateServerCommand(serverId, request.Name, request.IsAvailable));
                return result.ToApiResult();
            });

            group.MapDelete("/{serverId}",
                async ([FromRoute] Guid serverId, ISender sender) =>
            {
                Result result = await sender.Send(new DeleteServerCommand(serverId));
                return result.ToApiResult();
            });

            group.MapGet(string.Empty,
                async (ISender sender) =>
                {
                    var result = await sender.Send(new GetAllServersQuery());
                    return result.ToApiResult();
                });

            group.MapGet("/{serverId}",
                async ([FromRoute] Guid serverId, ISender sender) =>
                {
                    var result = await sender.Send(new GetServerByIdQuery(serverId));
                    return result.ToApiResult();
                });
        }
    }
}
