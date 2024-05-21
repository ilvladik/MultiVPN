﻿namespace IdentityServerApi.Options
{
    public class JwtOptions
    {
        public const string Name = "Jwt";

        public required string Issuer { get; set; }

        public required string Audience { get; set; }

        public required string Key { get; set; }
    }
}
