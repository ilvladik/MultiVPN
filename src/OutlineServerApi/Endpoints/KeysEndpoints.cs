using MediatR;
using Microsoft.AspNetCore.Mvc;
using OutlineServerApi.Application.Dtos.Requests.Keys;
using OutlineServerApi.Application.Dtos.Responses.Keys;
using OutlineServerApi.Application.Features.Commands.Keys;
using OutlineServerApi.Application.Features.Queries.Keys;
using OutlineServerApi.Endpoints.Extensions;
using SharedKernel.Results;
using System.Security.Claims;

namespace OutlineServerApi.Endpoints
{
    public static class KeysEndpoints
    {
        public static void MapKeysEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/keys");


            group.MapPost(string.Empty,
                async ([FromBody] CreateNewKeyRequest request, ISender sender) =>
            {
                Result<KeyResponse> result = await sender.Send(new CreateNewKeyCommand(request.Name, request.CountryId ?? Guid.Empty));
                return result.ToApiResult();
            })
                .RequireAuthorization("AuthenticatedUser");

            group.MapPut("server/{serverId}",
                async ([FromRoute] Guid serverId, [FromBody] TransferKeysRequest request, ISender sender) =>
            {
                Result result = await sender.Send(new TransferKeysCommand(serverId, request.ServerId ?? Guid.Empty));
                return result.ToApiResult();
            })
                .RequireAuthorization("AdminPolicy");

            group.MapPut("{keyId}/server", 
                async ([FromRoute] Guid keyId, [FromBody] TransferKeyToNewServerRequest request, ISender sender) =>
            {
                Result result = await sender.Send(new TransferKeyToNewServerCommand(keyId, request.ServerId ?? Guid.Empty));
                return result.ToApiResult();
            })
                .RequireAuthorization("AdminPolicy");

            group.MapPut("{keyId}",
                async ([FromRoute] Guid keyId, [FromBody] UpdateKeyRequest request, ISender sender) =>
            {
                Result result = await sender.Send(new UpdateKeyCommand(keyId, request.Name));
                return result.ToApiResult();
            })
                .RequireAuthorization("AuthenticatedUser");

            group.MapDelete("{keyId}",
                async ([FromRoute] Guid keyId, ISender sender) =>
            {
                Result result = await sender.Send(new DeleteKeyCommand(keyId));
                return result.ToApiResult();
            })
                .RequireAuthorization("AuthenticatedUser");

            group.MapGet(string.Empty, 
                async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllKeysQuery());
                return result.ToApiResult();
            })
                .RequireAuthorization("AuthenticatedUser");

            group.MapGet("{keyId}",
                async ([FromRoute] Guid keyId, ISender sender) =>
            {
                var result = await sender.Send(new GetKeyByIdQuery(keyId));
                return result.ToApiResult();
            })
                .RequireAuthorization("AuthenticatedUser");
            

            group.MapGet("/connect/{keyId}",
                async ([FromRoute] Guid keyId, HttpContext context, [FromQuery] string? type, ISender sender) =>
            {
                context.Response.Headers.AccessControlAllowOrigin = "*";
                context.Response.Headers.AccessControlAllowCredentials = "*";
                context.Response.Headers.AccessControlAllowMethods = "*";
                context.Response.Headers.AccessControlAllowHeaders = "*";
                var result = await sender.Send(new GetOutlineKeyByIdQuery(keyId));
                if (result.Succeeded)
                    return Results.Ok(result.Value);

                return result.ToApiResult();
            })
            .AllowAnonymous();
        }
    }
}
