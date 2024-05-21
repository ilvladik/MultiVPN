﻿namespace IdentityServerApi.Application.Dtos.Requests
{
    public class ResetPasswordRequest
    {
        public required string Email { get; init; }

        public required string ResetCode { get; init; }

        public required string NewPassword { get; init; }
    }
}
