namespace IdentityServerApi.Options
{
    public class ExternalLinksOptions 
    {
        public const string Name = "ExternalLinks";

        public required string ResetPasswordLink { get; set; }
    }
}
