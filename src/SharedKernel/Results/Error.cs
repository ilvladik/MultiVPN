namespace SharedKernel.Results
{
    /// <summary>
    /// Encapsulates an error.
    /// </summary>
    public sealed class Error
    {
        /// <summary>
        /// The code for this error.
        /// </summary>
        public required string Code { get; init; }

        /// <summary>
        /// The description for this error.
        /// </summary>
        public required string Description { get; init; }
    };
    
}
