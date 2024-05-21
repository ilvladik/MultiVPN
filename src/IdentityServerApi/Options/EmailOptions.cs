namespace IdentityServerApi.Options
{
    public class EmailOptions
    {
        public const string Name = "Email";

        public required string From { get; set; }

        public required string Smtp { get; set; }
        
        public required int Port {  get; set; }
        
        public required string Address { get; set; }
        
        public required string Password { get; set; }
    }
}
