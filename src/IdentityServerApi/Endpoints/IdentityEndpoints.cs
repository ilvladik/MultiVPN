using IdentityServerApi.Application.Dtos.Requests;
using IdentityServerApi.Application.Dtos.Responses;
using IdentityServerApi.Application.Features.Commands;
using IdentityServerApi.Application.Features.Queries;
using IdentityServerApi.Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Results;

namespace IdentityServerApi.Endpoints
{
    public static class IdentityEndpoints
    {
        public static void MapIdentityEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("");

            group.MapPost("/register", async ([FromBody] RegisterRequest request, [FromQuery] string? callbackUri, ISender sender) =>
            {
                Result result = await sender.Send(new RegisterCommand(request.Email, request.Password, callbackUri));
                return result.ToApiResult();
            });

            group.MapPost("/login",
                async ([FromBody] LoginRequest request, ISender sender) =>
            {
                Result<AccessTokenResponse> result = await sender.Send(new LoginCommand(request.Email, request.Password));
                return result.ToApiResult();
            });

            group.MapPost("/refresh", 
                async ([FromBody] RefreshRequest request, ISender sender) =>
            {
                Result<AccessTokenResponse> result = await sender.Send(new RefreshCommand(request.RefreshToken));
                return result.ToApiResult();
            });

            group.MapGet("/confirmEmail",
                async ([FromQuery] string userId, [FromQuery] string code, [FromQuery] string? callbackUri, ISender sender) =>
            {
                Result result = await sender.Send(new ConfirmEmailCommand(userId, code));
                if (callbackUri is not null)
                    return Results.Redirect(callbackUri);
                return result.ToApiResult();
            })
            .WithMetadata(new EndpointNameMetadata("confirmEmail")); ;

            group.MapPost("/resendConfirmationEmail", 
                async ([FromBody] ResendConfirmationEmailRequest request, ISender sender) =>
            {
                Result result = await sender.Send(new ResendConfirmationEmailCommand(request.Email));
                return result.ToApiResult();
            });

            group.MapPost("/forgotPassword", 
                async ([FromBody] ForgotPasswordRequest request, ISender sender) =>
            { 
                Result result = await sender.Send(new ForgotPasswordCommand(request.Email));
                return result.ToApiResult();
            });

            group.MapPost("/resetPassword", 
                async ([FromBody] ResetPasswordRequest request, ISender sender) =>
            {
                Result result = await sender.Send(new ResetPasswordCommand(request.Email, request.ResetCode, request.NewPassword));
                return result.ToApiResult();
            });

            group.MapGet("/account",
            async (ISender sender) =>
            {
                Result result = await sender.Send(new GetUserQuery());
                return result.ToApiResult();
            })
                .RequireAuthorization("AuthenticatedUser");
        }
    }
}
