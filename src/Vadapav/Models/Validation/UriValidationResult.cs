using System.ComponentModel.DataAnnotations;

namespace Vadapav.Models.Validation
{
    public class UriValidationResult : ValidationResult
    {
        public new bool Success { get; }

        public UriValidationResult(string? errorMessage)
            : base(errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                Success = true;
        }
    }
}
