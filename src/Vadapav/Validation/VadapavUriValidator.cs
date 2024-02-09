using Vadapav.Models.Validation;

namespace Vadapav.Validation
{
    public static class VadapavUriValidator
    {
        private static readonly UriValidationResult SuccessResult = new(null);
        private static readonly UriValidationResult UriParseErrorResult = new("Failed to parse URI.");
        private static readonly UriValidationResult HostnameErrorResult = new("This URI is not a valid, because the hostname does not match for vadapav.");

        public static UriValidationResult ValidateApiURL(string input)
        {
            if (!Uri.TryCreate(input, UriKind.Absolute, out var uri))
                return UriParseErrorResult;

            return ValidateApiURL(uri);
        }

        public static UriValidationResult ValidateApiURL(Uri uri)
        {
            if (!uri.DnsSafeHost.Contains("vadapav.mov"))
                return HostnameErrorResult;

            if (uri.Segments.Length != 2 && uri.Segments.Length != 3)
                return new UriValidationResult("This URI is not valid; there are to many or to less segments.");

            // handle 2 segments, which can only be root directory URI
            if (uri.Segments.Length == 2)
            {
                var firstSegment = uri.Segments[0];
                var secondSegment = uri.Segments[1].Trim('/');

                if (firstSegment != "/")
                    return new UriValidationResult("This URI is not valid; there are 2 segments in total so the first one must be '/'.");

                if (secondSegment != VadapavRouteProvider.DirectoryPathSpecifier)
                    return new UriValidationResult($"This URI is not valid; there are 2 segments in total so the last one must be the directory specifier '{VadapavRouteProvider.DirectoryPathSpecifier}'.");

                return SuccessResult;
            }

            if (uri.Segments.Length == 3)
            {
                var firstSegment = uri.Segments[0];
                var secondSegment = uri.Segments[1].Trim('/');
                var thirdSegment = uri.Segments[2];

                if (firstSegment != "/")
                    return new UriValidationResult("This URI is not valid; there are 3 segments in total so the first one must be '/'.");

                if (!VadapavRouteProvider.AllSpecifiers.Any(specifier => specifier.Equals(secondSegment)))
                    return new UriValidationResult($"This URI is not valid; there are 3 segments in total so the second one must be a known specifier.");

                if (!Guid.TryParse(thirdSegment, out var _))
                    return new UriValidationResult("This URI is not valid; there are 3 segments in total so the last one must be a valid GUID.");
            }

            return SuccessResult;
        }

        public static UriValidationResult ValidateDirectoryUri(string input)
        {
            if (!Uri.TryCreate(input, UriKind.Absolute, out var uri))
                return UriParseErrorResult;

            return ValidateDirectoryUri(uri);
        }

        public static UriValidationResult ValidateDirectoryUri(Uri uri)
        {
            if (!uri.DnsSafeHost.Contains("vadapav.mov"))
                return HostnameErrorResult;

            if (string.IsNullOrWhiteSpace(uri.Fragment))
                return new UriValidationResult("This URI is not valid; the fragment is null or empty.");

            if (!Guid.TryParse(uri.Fragment.Replace("#", string.Empty), out var _))
                return new UriValidationResult("This URI is not valid; the fragment must contain a valid GUID.");

            return SuccessResult;
        }
    }
}
