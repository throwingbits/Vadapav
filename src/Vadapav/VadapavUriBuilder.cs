using Vadapav.Models;

namespace Vadapav
{
    public class VadapavUriBuilder : IVadapavUriBuilder
    {
        private readonly Uri _baseAddress;

        private const string ApiBasePath = "/api";
        private const string RootDirectoryPath = "d";
        private const string DirectoryBasePath = "d";
        private const string FileBasePath = "f";
        private const string SearchBasePath = "s";

        public VadapavUriBuilder(string baseAddress)
            : this(new Uri(baseAddress))
        {
        }

        public VadapavUriBuilder(Uri baseAddress)
        {
            ArgumentNullException.ThrowIfNull(baseAddress);

            _baseAddress = baseAddress;
        }

        public string GetRootDirectoryUriString() =>
            GetRootDirectoryUri().ToString();

        public Uri GetRootDirectoryUri()
        {
            var builder = CreateBuilder();
            builder.Path = $"{ApiBasePath}/{RootDirectoryPath}";

            return builder.Uri;
        }

        public string GetDirectoryUriString(VadapavDirectory directory) =>
            GetDirectoryUriString(directory.Id);

        public string GetDirectoryUriString(Guid directoryId) =>
            GetDirectoryUriString(directoryId.ToString());

        public string GetDirectoryUriString(string directoryId) =>
            GetDirectoryUri(directoryId).ToString();

        public Uri GetDirectoryUri(VadapavDirectory directory) =>
            GetDirectoryUri(directory.Id);

        public Uri GetDirectoryUri(Guid directoryId) =>
            GetDirectoryUri(directoryId.ToString());

        public Uri GetDirectoryUri(string directoryId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(directoryId);

            var builder = CreateBuilder();
            builder.Path = $"{ApiBasePath}/{DirectoryBasePath}/{directoryId}";

            return builder.Uri;
        }

        public string GetFileUriString(VadapavFile file) =>
            GetFileUriString(file.Id);

        public string GetFileUriString(Guid fileId) =>
            GetFileUriString(fileId.ToString());

        public string GetFileUriString(string fileId) =>
            GetFileUri(fileId).ToString();

        public Uri GetFileUri(VadapavFile file) =>
            GetFileUri(file.Id);

        public Uri GetFileUri(Guid directoryId) =>
            GetFileUri(directoryId.ToString());

        public Uri GetFileUri(string fileId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fileId);

            var builder = CreateBuilder();
            builder.Path = $"{FileBasePath}/{fileId}";

            return builder.Uri;
        }

        public string GetSearchUriString(string searchTerm) =>
            GetSearchUri(searchTerm).ToString();

        public Uri GetSearchUri(string searchTerm)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(searchTerm);

            var builder = CreateBuilder();
            builder.Path = $"{SearchBasePath}/{searchTerm}";

            return builder.Uri;
        }

        private UriBuilder CreateBuilder()
        {
            var baseAddress = _baseAddress.ToString();
            var builder = new UriBuilder(baseAddress);

            return builder;
        }
    }
}
