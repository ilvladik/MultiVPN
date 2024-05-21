namespace OutlineServerApi.Core.Models
{
    /// <summary>
    /// Represents key with data to connect to outline service
    /// </summary>
    public class Key
    {

        /// <summary>
        /// Identifier of a key
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Name of a key
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Represents a many-to-one relationship with an <see cref="Models.Server"/> entity
        /// </summary>
        public required Guid ServerId { get; set; }

        public virtual Server? Server { get; set; }

        /// <summary>
        /// The identifier of user who created key.
        /// </summary>
        public required Guid CreatedByUser { get; set; }

        /// <summary>
        /// Represents internal identifier for identifying the key in the outline system
        /// </summary>
        public required string InternalId { get; set; }

        /// <summary>
        /// Password of outline key
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Port of outline key
        /// </summary>
        public required int Port { get; set; }

        /// <summary>
        /// Method of outline key
        /// </summary>
        public required string Method { get; set; }
    }
}
