using OutlineServerApi.Infrastructure.OutlineApi.Models;

namespace OutlineServerApi.Infrastructure.OutlineApi
{
    internal interface IOutlineApi
    {
        /// <summary>
        /// The method gets information about the outline server.
        /// </summary>
        /// <param name="serverAddress">Represents hostname, port and api prefix stored as a <see cref="Uri"/>.</param>
        /// <returns>An <see cref="OutlineServer"/> instance.</returns>
        Task<OutlineServer> GetServerAsync(Uri serverAddress);

        /// <summary>
        /// The method gets information about the all outline keys.
        /// </summary>
        /// <param name="serverAddress">Represents hostname, port and api prefix of <see cref="OutlineServer" /> stored as a <see cref="Uri"/>.</param>
        /// <returns>An <see cref="IEnumerator{T}"/> of <see cref="OutlineKey"/> instances.</returns>
        Task<IEnumerable<OutlineKey>> GetAllKeysAsync(Uri serverAddress);

        /// <summary>
        /// The method gets information about the outline key.
        /// </summary>
        /// <param name="serverAddress">Represents hostname, port and api prefix of <see cref="OutlineServer" /> stored as a <see cref="Uri"/>.</param>
        /// <param name="id">An identifier of key in outline system.</param>
        /// <returns>An <see cref="OutlineKey"/> instance.</returns>
        Task<OutlineKey> GetKeyByIdAsync(Uri serverAddress, string id);

        /// <summary>
        /// The method creates a new key on the server
        /// </summary>
        /// <param name="serverAddress">Represents hostname, port and api prefix of <see cref="OutlineServer" /> stored as a <see cref="Uri"/>.</param>
        /// <returns>An <see cref="OutlineKey"/> instance.</returns>
        Task<OutlineKey> CreateKeyAsync(Uri serverAddress);

        /// <summary>
        /// The method deletes existing key on the server.
        /// </summary>
        /// <param name="serverAddress">Represents hostname, port and api prefix of <see cref="OutlineServer" /> stored as a <see cref="Uri"/>.</param>
        /// <param name="id">An identifier of key in outline system.</param>
        Task DeleteKeyAsync(Uri serverAddress, string id);

        /// <summary>
        /// The method deletes key from <paramref name="source"/> and create new one on a <paramref name="dest"/>.
        /// </summary>
        /// <param name="source">Represents hostname, port and api prefix of old <see cref="OutlineServer" /> stored as a <see cref="Uri"/>.</param>
        /// <param name="dest">Represents hostname, port and api prefix of new <see cref="OutlineServer" /> stored as a <see cref="Uri"/>.</param>
        /// <param name="id">An identifier of key in outline system on <paramref name="source"/> server.</param>
        /// <returns>An new <see cref="OutlineKey"/> instance.</returns>
        Task<OutlineKey> TransferKeyToNewServer(Uri source, Uri dest, string id);

    }
}
