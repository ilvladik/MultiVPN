namespace OutlineServerApi.Core.Models
{
    /// <summary>
    /// Represents the country where the server is registered.
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Identifier of a country.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name of a country.
        /// </summary>
        public required string Name { get; set; }
    }
}
