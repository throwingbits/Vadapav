using System.Text.Json.Serialization;

namespace Vadapav.Models.Http
{
    internal class VadapavDataItem
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        [JsonPropertyName("dir")]
        public bool IsDirectory { get; set; }

        public long? Size { get; set; }

        public Guid? Parent { get; set; }

        [JsonPropertyName("mtime")]
        public DateTime ModifiedAt { get; set; }

        public List<VadapavDataItem>? Files { get; set; }
    }
}
