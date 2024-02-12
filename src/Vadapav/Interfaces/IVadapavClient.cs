using Vadapav.Models;

namespace Vadapav
{
    public interface IVadapavClient
    {
        /// <summary>
        /// Instance of a <see cref="IVadapavUriBuilder"/> which is used to build valid vadapav URIs.
        /// </summary>
        IVadapavUriBuilder UriBuilder { get; }

        /// <summary>
        /// Gets the root directory from vadapav.
        /// </summary>
        Task<VadapavDirectory> GetRootDirectoryAsync();

        /// <summary>
        /// Gets a specific directory from vadapav.
        /// </summary>
        Task<VadapavDirectory> GetDirectoryAsync(VadapavDirectory directory);

        /// <summary>
        /// Gets a specific directory from vadapav.
        /// </summary>
        /// <param name="directoryId">The id of the directory.</param>
        Task<VadapavDirectory> GetDirectoryAsync(Guid directoryId);

        /// <summary>
        /// Gets a specific directory from vadapav.
        /// </summary>
        /// <param name="directoryId">The id of the directory.</param>
        Task<VadapavDirectory> GetDirectoryAsync(string directoryId);

        /// <summary>
        /// Gets a specific file from vadapav.
        /// </summary>
        Task<(string Name, Stream ContentStream)> GetFileAsync(VadapavFile file);

        /// <summary>
        /// Gets a specific file from vadapav.
        /// </summary>
        /// <param name="fileId">The id of the file.</param>
        Task<(string Name, Stream ContentStream)> GetFileAsync(Guid fileId);

        /// <summary>
        /// Gets a specific file from vadapav.
        /// </summary>
        /// <param name="fileId">The id of the file.</param>
        Task<(string Name, Stream ContentStream)> GetFileAsync(string fileId);

        /// <summary>
        /// Gets a chunk of a specific file from vadapav.
        /// </summary>
        /// <param name="from">The start index of the chunk.</param>
        /// <param name="to">The end index of the chunk.</param>
        Task<(string Name, Stream ContentStream)> GetFileRangeAsync(
            VadapavFile file,
            long from,
            long? to);

        /// <summary>
        /// Gets a chunk of a specific file from vadapav.
        /// </summary>
        /// <param name="fileId">The id of the file.</param>
        /// <param name="from">The start index of the chunk.</param>
        /// <param name="to">The end index of the chunk.</param>
        /// <returns></returns>
        Task<(string Name, Stream ContentStream)> GetFileRangeAsync(
            Guid fileId,
            long from,
            long? to);

        /// <summary>
        /// Gets a chunk of a specific file from vadapav.
        /// </summary>
        /// <param name="fileId">The id of the file.</param>
        /// <param name="from">The start index of the chunk.</param>
        /// <param name="to">The end index of the chunk.</param>
        /// <returns></returns>
        Task<(string Name, Stream ContentStream)> GetFileRangeAsync(
            string fileId,
            long from,
            long? to);

        /// <summary>
        /// Downloads a specific from vadapav to the given path.
        /// </summary>
        /// <param name="path">The target path for the file.</param>
        /// <param name="resume">Flag to specify if a download should be resumed when the file is already present.</param>
        /// <returns></returns>
        Task DownloadDirectoryAsync(
            VadapavDirectory directory,
            string path,
            bool resume = true);

        /// <summary>
        /// Downloads a specific from vadapav to the given path.
        /// </summary>
        /// <param name="directoryId">The id of the file.</param>
        /// <param name="path">The target path for the file.</param>
        /// <param name="resume">Flag to specify if a download should be resumed when the file is already present.</param>
        /// <returns></returns>
        Task DownloadDirectoryAsync(
            Guid directoryId,
            string path,
            bool resume = true);

        /// <summary>
        /// Downloads a specific from vadapav to the given path.
        /// </summary>
        /// <param name="directoryId">The id of the file.</param>
        /// <param name="path">The target path for the file.</param>
        /// <param name="resume">Flag to specify if a download should be resumed when the file is already present.</param>
        /// <returns></returns>
        Task DownloadDirectoryAsync(
            string directoryId,
            string path,
            bool resume = true);

        /// <summary>
        /// Downloads a specific from vadapav to the given path.
        /// </summary>
        /// <param name="path">The target path for the file.</param>
        /// <param name="resume">Flag to specify if a download should be resumed when the file is already present.</param>
        /// <returns></returns>
        Task DownloadFileAsync(
            VadapavFile file,
            string path,
            bool resume = true);

        /// <summary>
        /// Downloads a specific from vadapav to the given path.
        /// </summary>
        /// <param name="fileId">The id of the file.</param>
        /// <param name="path">The target path for the file.</param>
        /// <param name="resume">Flag to specify if a download should be resumed when the file is already present.</param>
        /// <returns></returns>
        Task DownloadFileAsync(
            Guid fileId,
            string path,
            bool resume = true);

        /// <summary>
        /// Downloads a specific from vadapav to the given path.
        /// </summary>
        /// <param name="fileId">The id of the file.</param>
        /// <param name="path">The target path for the file.</param>
        /// <param name="resume">Flag to specify if a download should be resumed when the file is already present.</param>
        /// <returns></returns>
        Task DownloadFileAsync(
            string fileId,
            string path,
            bool resume = true);

        /// <summary>
        /// Searches vadapav for directories which match the given search term.
        /// </summary>
        Task<VadapavSearchResults> SearchAsync(string searchTerm);

        IVadapavDirectoryIterator CreateDirectoryIterator(VadapavDirectory directory);

        IVadapavDirectoryIterator CreateDirectoryIterator(Guid directoryId);

        IVadapavDirectoryIterator CreateDirectoryIterator(string directoryId);
    }
}