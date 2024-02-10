namespace Vadapav
{
    public class VadapavUriBuilder : IVadapavUriBuilder
    {
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

            RootDirectoryUri = CreateApiUriWithParameter(
                VadapavRouteProvider.DirectoryPathSpecifier,
                string.Empty);
        }

        public Uri GetUriForDirectory(string directoryId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(directoryId);

            if (!Guid.TryParse(directoryId, out var _))
                throw new ArgumentException($"Invalid argument: '{nameof(directoryId)}' must be a valid GUID.");

            return CreateApiUriWithParameter(
                VadapavRouteProvider.DirectoryPathSpecifier,
                directoryId);
        }

        public Uri GetUriForFile(string fileId)
        {
            if (!Guid.TryParse(fileId, out var _))
                throw new ArgumentException($"Invalid argument: '{nameof(fileId)}' must be a valid GUID.");

            return CreateUriWithParameter(
                VadapavRouteProvider.FilePathSpecifier,
                fileId);
        }

        public Uri GetUriForSearch(string searchTerm)
        {
            return CreateApiUriWithParameter(
                VadapavRouteProvider.SearchPathSpecifier,
                searchTerm);
        }

        private Uri CreateApiUriWithParameter(string path, string parameter)
        {
            return CreateUriWithParameter(
                $"{VadapavRouteProvider.ApiPath}/{path}",
                parameter);
        }

        private Uri CreateUriWithParameter(string path, string parameter)
        {
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