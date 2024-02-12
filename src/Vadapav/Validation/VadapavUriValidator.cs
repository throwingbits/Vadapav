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

            if (uri.Segments.Length != 3 && uri.Segments.Length != 4)
                return new UriValidationResult("Invalid URI: insufficient segments, required are 3 or 4 segments.");

            var firstSegment = uri.Segments.First();
            var secondSegment = uri.Segments[1].Trim('/');
            var thirdSegment = uri.Segments[2].Trim('/');

            if (uri.Segments.Length == 3)
            {
                return HandleThreeSegmentApiUri(
                    firstSegment,
                    secondSegment,
                    thirdSegment);
            }

            var forthSegment = uri.Segments[3].Trim('/');

            return HandleFourSegmentApiUri(
                firstSegment,
                secondSegment,
                thirdSegment,
                forthSegment);
        }

        private static UriValidationResult HandleFourSegmentApiUri(params string[] segments)
        {
            var firstSegment = segments[0];
            var secondSegment = segments[1];
            var thirdSegment = segments[2];
            var forthSegment = segments[3];

            if (firstSegment != "/")
                return new UriValidationResult("Invalid URI: first segment must be '/'.");

            if (secondSegment != VadapavRouteProvider.ApiPath.Trim('/'))
                return new UriValidationResult("Invalid URI: second segment must be 'api'.");

            if (!VadapavRouteProvider.AllSpecifiers.Any(specifier => specifier.Equals(thirdSegment)))
                return new UriValidationResult("Invalid URI: third segment must be a known specifier.");

            if (thirdSegment != VadapavRouteProvider.SearchPathSpecifier)
            {
                if (!Guid.TryParse(forthSegment, out var _))
                    return new UriValidationResult("Invalid URI: forth segment must be a valid GUID.");
            }

            return SuccessResult;
        }


        private static UriValidationResult HandleThreeSegmentApiUri(params string[] segments)
        {
            var firstSegment = segments[0];
            var secondSegment = segments[1];
            var thirdSegment = segments[2];

            if (firstSegment != "/")
                return new UriValidationResult("Invalid URI: first segment must be '/'.");

            // must be file uri
            if (secondSegment == VadapavRouteProvider.FilePathSpecifier)
            {
                if (!Guid.TryParse(thirdSegment, out var _))
                    return new UriValidationResult("Invalid URI: third segment must be a valid GUID.");
            }
            // must be root directory uri
            else if (secondSegment == VadapavRouteProvider.ApiPath.Trim('/'))
            {
                if (thirdSegment != VadapavRouteProvider.DirectoryPathSpecifier)
                    return new UriValidationResult("Invalid URI: third segment must be the directory specifier.");
            }
            else
            {
                return new UriValidationResult("Invalid URI: failed to map segments.");
            }

            return SuccessResult;
        }

        public static UriValidationResult ValidatePageUri(string input)
        {
            ArgumentException.ThrowIfNullOrEmpty(input);

            if (!Uri.TryCreate(input, UriKind.Absolute, out var uri))
                return UriParseErrorResult;

            return ValidatePageUri(uri);
        }

        public static UriValidationResult ValidatePageUri(Uri uri)
        {
            ArgumentNullException.ThrowIfNull(uri);

            if (!uri.DnsSafeHost.Contains("vadapav.mov"))
                return HostnameErrorResult;

            if (uri.Segments.Length < 2)
                return new UriValidationResult("Invalid URI: insufficient segments, there must be 2");

            if (uri.Segments.Length > 2)
                return new UriValidationResult("Invalid URI: too many segments, there must be 2");

            var firstSegment = uri.Segments.First();
            var lastSegment = uri.Segments.Last().TrimEnd('/');

            if (firstSegment != "/")
                return new UriValidationResult("Invalid URI: first segment must be '/'.");

            if (!Guid.TryParse(lastSegment, out var _))
                return new UriValidationResult("Invalid URI: last segment must be a valid GUID.");

            return SuccessResult;
        }
    }
}
