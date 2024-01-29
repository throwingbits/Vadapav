namespace Vadapav.Models
{
    public class VadapavSearchResults
    {
        public long Count { get; set; } = 0;
        public List<VadapavDirectory> Directories { get; set; } = [];
        public List<VadapavFile> Files { get; set; } = [];
    }
}
