namespace Vadapav
{
    public interface IVadapavUriBuilder
    {
        Uri RootDirectoryUri { get; }
        Uri GetUriForDirectory(string directoryId);
        Uri GetUriForFile(string fileId);
        Uri GetUriForSearch(string searchTerm);
    }
}