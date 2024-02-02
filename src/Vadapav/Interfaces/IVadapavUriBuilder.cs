using Vadapav.Models;

namespace Vadapav
{
    public interface IVadapavUriBuilder
    {
        Uri GetDirectoryUri(Guid directoryId);
        Uri GetDirectoryUri(string directoryId);
        Uri GetDirectoryUri(VadapavDirectory directory);
        string GetDirectoryUriString(Guid directoryId);
        string GetDirectoryUriString(string directoryId);
        string GetDirectoryUriString(VadapavDirectory directory);
        Uri GetFileUri(Guid directoryId);
        Uri GetFileUri(string fileId);
        Uri GetFileUri(VadapavFile file);
        string GetFileUriString(Guid fileId);
        string GetFileUriString(string fileId);
        string GetFileUriString(VadapavFile file);
        Uri GetRootDirectoryUri();
        string GetRootDirectoryUriString();
        Uri GetSearchUri(string searchTerm);
        string GetSearchUriString(string searchTerm);
    }
}