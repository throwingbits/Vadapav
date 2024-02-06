using Vadapav.Models;

namespace Vadapav
{
    public partial class VadapavClient
    {
        /// <inheritdoc/>
        public IVadapavDirectoryIterator CreateDirectoryIterator(VadapavDirectory directory) =>
            CreateDirectoryIterator(directory.Id.ToString());

        /// <inheritdoc/>
        public IVadapavDirectoryIterator CreateDirectoryIterator(Guid directoryId) =>
            CreateDirectoryIterator(directoryId.ToString());

        /// <inheritdoc/>
        public IVadapavDirectoryIterator CreateDirectoryIterator(string directoryId) =>
            new VadapavDirectoryIterator(this, directoryId);
    }
}
