using Vadapav.Models;

namespace Vadapav
{
    internal class VadapavDirectoryIterator : IVadapavDirectoryIterator
    {
        private readonly IVadapavClient _client;
        private readonly string _directoryId;

        internal VadapavDirectoryIterator(
            IVadapavClient client,
            VadapavDirectory directory)

            : this(client, directory.Id)
        {
        }

        internal VadapavDirectoryIterator(
            IVadapavClient client,
            Guid directoryId)
            
            : this(client, directoryId.ToString())
        {
        }

        internal VadapavDirectoryIterator(
            IVadapavClient client,
            string directoryId)
        {
            _client = client;
            _directoryId = directoryId;
        }

        public Task<VadapavDirectory> IterateAsync()
        {
            return IterateInternalAsync(_directoryId);
        }

        private async Task<VadapavDirectory> IterateInternalAsync(string directoryId)
        {
            var currentDirectory = await _client.GetDirectoryAsync(directoryId);
            var currentDirectoryDirectories = new List<VadapavDirectory>(currentDirectory.Directories);

            foreach (var directory in currentDirectoryDirectories)
            {
                var dir = await IterateInternalAsync(directory.Id.ToString());
                currentDirectory.Directories.RemoveAll(d => d.Id == dir.Id);
                currentDirectory.Directories.Add(dir);
            }

            return currentDirectory;
        }
    }
}
