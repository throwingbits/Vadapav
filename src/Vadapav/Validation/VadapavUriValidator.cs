using System.ComponentModel.DataAnnotations;

namespace Vadapav.Validation
{
    public class VadapavUriValidationResult : ValidationResult
    {
        public bool HasErrors { get; }

        public VadapavUriValidationResult(string? errorMessage)
            : base(errorMessage)
        {
            if (!string.IsNullOrWhiteSpace(errorMessage))
                HasErrors = true;
        }
    }

    public static class VadapavUriValidator
    {
        

        public static VadapavUriValidationResult IsValidApiURL(string input)
        {
            if (!Uri.TryCreate(input, UriKind.Absolute, out var uri))
                return new VadapavUriValidationResult($"The value of parameter '{nameof(input)}' is not a valid URI.");

            return ValidateApiUri(uri);
        }

        public static VadapavUriValidationResult ValidateApiUri(Uri uri)
        {
            var result = ValidateHostname(uri);

            if (result != null)
                return result;


        }

        public static VadapavUriValidationResult ValidatePageUri(string input)
        {
            if (!Uri.TryCreate(input, UriKind.Absolute, out var uri))
                return new VadapavUriValidationResult($"The value of parameter '{nameof(input)}' is not a valid URI.");

            return ValidatePageUri(uri);
        }

        public static VadapavUriValidationResult ValidatePageUri(Uri uri)
        {
            var result = ValidateHostname(uri);

            if (result != null)
                return result;
        }

        private static VadapavUriValidationResult? ValidateHostname(Uri uri)
        {
            if (!uri.DnsSafeHost.Contains("vadapav.mov"))
                return new VadapavUriValidationResult("The hostname does not match ");

            return null;
        }
            
    }
}
