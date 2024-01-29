namespace Vadapav.Models
{
    public abstract class VadapavItem
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? Parent { get; set; }
        public DateTime ModifiedAt { get; set; }

        protected VadapavItem() { }
    }
}
