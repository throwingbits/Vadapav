using System.ComponentModel.DataAnnotations;

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
            RootDirectoryUri = CreateUriBuilder(VadapavRouteProvider.DirectoryPathSpecifier).Uri;
        }

        public Uri GetUriForDirectory(string directoryId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(directoryId);

            return CreateApiUriWithParameter(
                VadapavRouteProvider.DirectoryPathSpecifier,
                directoryId);
        }

        public Uri GetUriForFile(string fileId)
        {
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

        public ValidationResult Validate(Uri uri)
        {
            if (!uri.DnsSafeHost.Contains("vadapav.mov"))
                return new ValidationResult("That's not a valid vadapav URL.");

            if (string.IsNullOrWhiteSpace(uri.Fragment))
                return new ValidationResult("URI does not ");

            if (!Guid.TryParse(uri.Fragment.Replace("#", string.Empty), out _))
                return new ValidationResult("That's not a valid vadapav directory URL, because the directory id is not a GUID.");

            return ValidationResult.Success!;
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