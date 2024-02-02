using Vadapav.Models;

namespace Vadapav
{
    public partial class VadapavClient
    {
        public IVadapavDirectoryIterator CreateDirectoryIterator(VadapavDirectory directory) =>
            CreateDirectoryIterator(directory.Id.ToString());

        public IVadapavDirectoryIterator CreateDirectoryIterator(Guid directoryId) =>
            CreateDirectoryIterator(directoryId.ToString());

        public IVadapavDirectoryIterator CreateDirectoryIterator(string directoryId) =>
            new VadapavDirectoryIterator(this, directoryId);
    }
}
