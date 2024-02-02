namespace Vadapav.Models.Iteration
{
    public class DirectoryIterationItem<TVadapavModel>
        where TVadapavModel : VadapavItem
    {
        public string Path { get; set; }
    }

    public class DirectoryIterationResult
    {
        public int Count { get; set; } = 0;

        // public List<DirectoryIterationItem> Items { get; set; } = [];
    }
}
