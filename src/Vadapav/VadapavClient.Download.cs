using Vadapav.Models;

namespace Vadapav
{
    public partial class VadapavClient
    {
        /// <inheritdoc/>
        public Task DownloadDirectoryAsync(VadapavDirectory directory, string path, bool resume = true)
        {
            ArgumentNullException
                .ThrowIfNull(directory);

            return DownloadDirectoryAsync(directory.Id, path);
        }

        /// <inheritdoc/>
        public Task DownloadDirectoryAsync(Guid directoryId, string path, bool resume = true)
        {
            return DownloadDirectoryAsync(directoryId.ToString(), path);
        }

        /// <inheritdoc/>
        public async Task DownloadDirectoryAsync(string directoryId, string path, bool resume = true)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(directoryId);

            ArgumentException
                .ThrowIfNullOrWhiteSpace(path);

            var directory = await GetDirectoryAsync(directoryId);

            Directory.CreateDirectory(path);

            foreach (var file in directory.Files)
            {
                if (string.IsNullOrWhiteSpace(file.Name))
                    continue;

                var filePath = Path.Combine(path, file.Name);
                await DownloadFileAsync(file.Id, filePath, resume);
            }

            foreach (var subDirectory in directory.Directories)
            {
                if (string.IsNullOrWhiteSpace(subDirectory.Name))
                    continue;

                var directoryPath = Path.Combine(path, subDirectory.Name);
                await DownloadDirectoryAsync(subDirectory.Id, directoryPath, resume);
            }
        }

        /// <inheritdoc/>
        public Task DownloadFileAsync(VadapavFile file, string path, bool resume = true)
        {
            ArgumentNullException
                .ThrowIfNull(file);

            return DownloadFileAsync(file.Id, path);
        }

        /// <inheritdoc/>
        public Task DownloadFileAsync(Guid fileId, string path, bool resume = true)
        {
            return DownloadFileAsync(fileId.ToString(), path);
        }

        /// <inheritdoc/>
        public async Task DownloadFileAsync(string fileId, string path, bool resume = true)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(fileId);

            ArgumentException
                .ThrowIfNullOrWhiteSpace(path);

            (string Name, Stream ContentStream) file;

            if (File.Exists(path))
            {
                if (!resume)
                    throw new IOException("The file already exists and the resume parameter is set to: false");

                using var localFile = File.OpenRead(path);
                var localFileLength = localFile.Length;

                file = await GetFileRangeAsync(fileId, localFileLength, null);
            }
            else
            {
                file = await GetFileAsync(fileId);
            }

            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                await file.ContentStream.CopyToAsync(fs);
            }
        }
    }
}
