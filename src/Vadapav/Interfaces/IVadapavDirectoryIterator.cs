using Vadapav.Models;

namespace Vadapav
{
    public interface IVadapavDirectoryIterator
    {
        Task<VadapavDirectory> IterateAsync();
    }
}
