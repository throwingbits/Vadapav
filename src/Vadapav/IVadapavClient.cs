using Vadapav.Models;

namespace Vadapav
{
    public interface IVadapavClient
    {
        Task<VadapavDirectory> GetRootDirectoryAsync();
        Task<Stream> GetFileAsync(VadapavFile file);
        Task<Stream> GetFileAsync(string id);
        Task<Stream> GetFileAsync(Guid id);
        Task<VadapavDirectory> GetDirectoryAsync(VadapavDirectory directory);
        Task<VadapavDirectory> GetDirectoryAsync(string id);
        Task<VadapavDirectory> GetDirectoryAsync(Guid id);
        Task<VadapavSearchResults> SearchAsync(string searchTerm);
        Task<List<VadapavFile>> SearchFilesAsync(string searchTerm);
        Task<List<VadapavDirectory>> SearchDirectoriesAsync(string searchTerm);
    }
}