namespace Vadapav.Models
{
    public class VadapavDirectory : VadapavItem
    {
        public List<VadapavDirectory> Directories { get; set; } = [];
        public List<VadapavFile> Files { get; set; } = [];
    }
}
