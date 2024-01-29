using System.Text.Json.Serialization;

namespace Vadapav.Models.Http
{
    internal class VadapavResponse<TData>
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public TData Data { get; set; }
    }
}
