using Vadapav.Models;

namespace Vadapav
{
    public interface IVadapavClient
    {
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
        /// <param name="id">The id of the directory.</param>
        Task<VadapavDirectory> GetDirectoryAsync(Guid id);

        /// <summary>
        /// Gets a specific directory from vadapav.
        /// </summary>
        /// <param name="id">The id of the directory.</param>
        Task<VadapavDirectory> GetDirectoryAsync(string id);

        /// <summary>
        /// Gets a specific file from vadapav.
        /// </summary>
        Task<(string Name, Stream ContentStream)> GetFileAsync(VadapavFile file);

        /// <summary>
        /// Gets a specific file from vadapav.
        /// </summary>
        /// <param name="id">The id of the file.</param>
        Task<(string Name, Stream ContentStream)> GetFileAsync(Guid id);

        /// <summary>
        /// Gets a specific file from vadapav.
        /// </summary>
        /// <param name="id">The id of the file.</param>
        Task<(string Name, Stream ContentStream)> GetFileAsync(string id);

        /// <summary>
        /// Gets a chunk of a specific file from vadapav.
        /// </summary>
        /// <param name="from">The start index of the chunk.</param>
        /// <param name="to">The end index of the chunk.</param>
        Task<(string Name, Stream ContentStream)> GetFileRangeAsync(
            VadapavFile file,
            long from,
            long to);

        /// <summary>
        /// Gets a chunk of a specific file from vadapav.
        /// </summary>
        /// <param name="id">The id of the file.</param>
        /// <param name="from">The start index of the chunk.</param>
        /// <param name="to">The end index of the chunk.</param>
        /// <returns></returns>
        Task<(string Name, Stream ContentStream)> GetFileRangeAsync(
            Guid id,
            long from,
            long to);

        /// <summary>
        /// Gets a chunk of a specific file from vadapav.
        /// </summary>
        /// <param name="id">The id of the file.</param>
        /// <param name="from">The start index of the chunk.</param>
        /// <param name="to">The end index of the chunk.</param>
        /// <returns></returns>
        Task<(string Name, Stream ContentStream)> GetFileRangeAsync(
            string id,
            long from,
            long to);

        /// <summary>
        /// Searches vadapav for elements which match the given search term.
        /// </summary>
        Task<VadapavSearchResults> SearchAsync(string searchTerm);

        /// <summary>
        /// Searches vadapav for files which match the given search term.
        /// </summary>
        Task<List<VadapavFile>> SearchFilesAsync(string searchTerm);

        /// <summary>
        /// Searches vadapav for directories which match the given search term.
        /// </summary>
        Task<List<VadapavDirectory>> SearchDirectoriesAsync(string searchTerm);
    }
}