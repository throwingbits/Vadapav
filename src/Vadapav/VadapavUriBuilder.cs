namespace Vadapav
{
    public class VadapavUriBuilder : IVadapavUriBuilder
    {
        private const string ApiBasePath = "/api";
        private const string RootDirectoryPath = "d";
        private const string DirectoryBasePath = "d";
        private const string FileBasePath = "f";
        private const string SearchBasePath = "s";

        private readonly Uri _baseAddress;

        public Uri RootDirectoryUri { get; }

        public VadapavUriBuilder(string baseAddress)
            : this(new Uri(baseAddress))
        {
        }

        public VadapavUriBuilder(Uri baseAddress)
        {
            ArgumentNullException.ThrowIfNull(baseAddress);

            _baseAddress = baseAddress;
            RootDirectoryUri = CreateUriBuilder(RootDirectoryPath).Uri;
        }

        public Uri GetUriForDirectory(string directoryId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(directoryId);

            return CreateApiUriWithParameter(
                DirectoryBasePath,
                directoryId);
        }

        public Uri GetUriForFile(string fileId)
        {
            return CreateUriWithParameter(
                FileBasePath,
                fileId);
        }

        public Uri GetUriForSearch(string searchTerm)
        {
            return CreateApiUriWithParameter(
                SearchBasePath,
                searchTerm);
        }

        private Uri CreateApiUriWithParameter(string path, string parameter)
        {
            return CreateUriWithParameter(
                $"{ApiBasePath}/{path}",
                parameter);
        }

        private Uri CreateUriWithParameter(string path, string parameter)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(parameter);
            ArgumentException.ThrowIfNullOrWhiteSpace(path);

            var builder = CreateUriBuilder(path);
            builder.Path = $"{builder.Path}{parameter}";

            return builder.Uri;
        }

        private UriBuilder CreateUriBuilder(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);

            if (!path.StartsWith('/'))
                path = $"/{path}";

            if (!path.EndsWith('/'))
                path = $"{path}/";

            var builder = new UriBuilder(_baseAddress)
            {
                Path = path
            };

            return builder;
        }
    }
}