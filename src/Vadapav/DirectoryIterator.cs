using Vadapav.Models;
using Vadapav.Models.Iteration;

namespace Vadapav
{
    public interface IDirectoryIterator
    {
        Task<DirectoryIterationResult> IterateAsync();
    }

    internal class DirectoryIterator
    {
        private readonly VadapavDirectory _directory;

        public DirectoryIterator(VadapavDirectory directory)
        {
            _directory = directory;
        }
    }
}
