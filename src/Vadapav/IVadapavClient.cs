using Vadapav.Models;

namespace Vadapav
{
    public interface IVadapavClient
    {
        Task<VadapavDirectory> GetRootDirectoryAsync();
        Task<(string Name, Stream ContentStream)> GetFileAsync(VadapavFile file);
        Task<(string Name, Stream ContentStream)> GetFileAsync(string id);
        Task<(string Name, Stream ContentStream)> GetFileAsync(Guid id);
        Task<VadapavDirectory> GetDirectoryAsync(VadapavDirectory directory);
        Task<VadapavDirectory> GetDirectoryAsync(string id);
        Task<VadapavDirectory> GetDirectoryAsync(Guid id);
        Task<VadapavSearchResults> SearchAsync(string searchTerm);
        Task<List<VadapavFile>> SearchFilesAsync(string searchTerm);
        Task<List<VadapavDirectory>> SearchDirectoriesAsync(string searchTerm);
    }
}