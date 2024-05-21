namespace OutlineServerApi.Core.Models
{
    /// <summary>
    /// Represents the server with data to connect to outline servers.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Identifier of a server.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name of a server.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Flag indicating whether if server available or not.
        /// </summary>
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// IP address or domain name that is used to connect to the outline server
        /// </summary>
        public required string Hostname { get; set; }

        /// <summary>
        /// Port that is used to connect to the outline server 
        /// </summary>
        public required int Port { get; set; }

        /// <summary>
        /// Api Prefix that is used to connect to the outline server
        /// </summary>
        /// <value>A randomly generated URI path used for security purposes</value>
        public required string ApiPrefix { get; set; }

        /// <summary>
        /// Represents a one-to-many relationship with an <see cref="Models.Country"/> entity
        /// </summary>
        public required Guid CountryId { get; set; }

        public virtual Country? Country { get; set; }

        /// <summary>
        /// Represents a many-to-one relationship with an <see cref="Key"/> entity
        /// </summary>
        public virtual ICollection<Key> Keys { get; set; } = [];
    }
}
