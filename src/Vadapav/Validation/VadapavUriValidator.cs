using Vadapav.Models.Validation;

namespace Vadapav.Validation
{
    public static class VadapavUriValidator
    {
        private static readonly UriValidationResult SuccessResult = new(null);
        private static readonly UriValidationResult UriParseErrorResult = new("Invalid URI: Failed to parse.");
        private static readonly UriValidationResult HostnameErrorResult = new("Invalid URI: Hostname does not match for vadapav.");

        public static UriValidationResult ValidateApiURL(string input)
        {
            ArgumentException.ThrowIfNullOrEmpty(input);

            if (!Uri.TryCreate(input, UriKind.Absolute, out var uri))
                return UriParseErrorResult;

            return ValidateApiURL(uri);
        }

        public static UriValidationResult ValidateApiURL(Uri uri)
        {
            ArgumentNullException.ThrowIfNull(uri);

            if (!uri.DnsSafeHost.Contains("vadapav.mov"))
                return HostnameErrorResult;

            if (uri.Segments.Length != 2 && uri.Segments.Length != 3)
                return new UriValidationResult("Invalid URI: insufficient segments, required are 2 or 3 segments.");

            // handle 2 segments, which can only be root directory URI
            if (uri.Segments.Length == 2)
            {
                var firstSegment = uri.Segments[0];
                var secondSegment = uri.Segments[1].Trim('/');

                if (firstSegment != "/")
                    return new UriValidationResult("Invalid URI: first segment must be '/' when 2 segments are provided.");

                if (secondSegment != VadapavRouteProvider.DirectoryPathSpecifier)
                    return new UriValidationResult("Invalid URI: last segment must be the directory specifier when 2 segments are provided.");

                return SuccessResult;
            }

            if (uri.Segments.Length == 3)
            {
                var firstSegment = uri.Segments[0];
                var secondSegment = uri.Segments[1].Trim('/');
                var thirdSegment = uri.Segments[2];

                if (firstSegment != "/")
                    return new UriValidationResult("Invalid URI: first segment must be '/' when 3 segments are provided.");

                if (!VadapavRouteProvider.AllSpecifiers.Any(specifier => specifier.Equals(secondSegment)))
                    return new UriValidationResult("Invalid URI: second segment must be a known specifier when 3 segments are provided.");

                if (!Guid.TryParse(thirdSegment, out var _) && secondSegment != VadapavRouteProvider.SearchPathSpecifier)
                    return new UriValidationResult("Invalid URI: last segment must be a valid GUID when 3 segments are provided.");
            }

            return SuccessResult;
        }

        public static UriValidationResult ValidateDirectoryUri(string input)
        {
            ArgumentException.ThrowIfNullOrEmpty(input);

            if (!Uri.TryCreate(input, UriKind.Absolute, out var uri))
                return UriParseErrorResult;

            return ValidateDirectoryUri(uri);
        }

        public static UriValidationResult ValidateDirectoryUri(Uri uri)
        {
            ArgumentNullException.ThrowIfNull(uri);

            if (!uri.DnsSafeHost.Contains("vadapav.mov"))
                return HostnameErrorResult;

            if (string.IsNullOrWhiteSpace(uri.Fragment))
                return new UriValidationResult("Invalid URI: fragment can't be null or empty.");

            if (!Guid.TryParse(uri.Fragment.Replace("#", string.Empty), out var _))
                return new UriValidationResult("Invalid URI: fragment must be a valid GUID.");

            return SuccessResult;
        }
    }
}
