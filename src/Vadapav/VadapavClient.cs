using System.Net.Http.Json;

using Vadapav.Http;
using Vadapav.Models;
using Vadapav.Models.Http;

namespace Vadapav
{
    public class VadapavClient : IVadapavClient
    {
        private readonly HttpClient _client;

        public VadapavClient(string baseAddress, ushort maxRetryCount = 5)
            : this(new Uri(baseAddress), maxRetryCount)
        {
        }

        public VadapavClient(Uri baseAddress, ushort maxRetryCount = 5)
        {
            _client = new HttpClient(new HttpRetryMessageHandler(maxRetryCount))
            {
                BaseAddress = baseAddress
            };
        }

        public VadapavClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentException(null, nameof(client));
        }

        public async Task<VadapavDirectory> GetRootDirectoryAsync()
        {
            var response = await _client.GetFromJsonAsync<VadapavDirectoryResponse>(EndPointProvider.Root) ??
                throw new InvalidOperationException("Failed to get root directory.");

            return response.Data.AsDirectory();
        }

        public Task<VadapavDirectory> GetDirectoryAsync(VadapavDirectory directory)
        {
            ArgumentNullException
                .ThrowIfNull(directory);

            return GetDirectoryAsync(directory.Id);
        }

        public Task<VadapavDirectory> GetDirectoryAsync(Guid id)
        {
            return GetDirectoryAsync(id.ToString());
        }

        public async Task<VadapavDirectory> GetDirectoryAsync(string id)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(id);

            var endpoint = EndPointProvider.Create(
                EndPointProvider.Directory,
                id);

            var response = await _client.GetFromJsonAsync<VadapavDirectoryResponse>(endpoint) ??
                throw new InvalidOperationException($"Failed to get directory '{id}'.");

            return response.Data.AsDirectory();
        }

        public Task<(string Name, Stream ContentStream)> GetFileAsync(VadapavFile file)
        {
            ArgumentNullException
                .ThrowIfNull(file);

            return GetFileAsync(file.Id);
        }

        public Task<(string Name, Stream ContentStream)> GetFileAsync(Guid id)
        {
            return GetFileAsync(id.ToString());
        }

        public async Task<(string Name, Stream ContentStream)> GetFileAsync(string id)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(id);

            var requestUri = EndPointProvider.Create(
                EndPointProvider.File,
                id);

            var response = await _client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            if (!response.Content.Headers.TryGetValues("Content-Disposition", out var contentDispositionValues))
                throw new InvalidOperationException("");

            var contentDisposition = contentDispositionValues.First();

            // this is ugly as hell but works at the moment. We should fix that at backend level
            var fileName = contentDisposition.Replace("attachment; filename=", string.Empty);

            if (string.IsNullOrWhiteSpace(fileName))
                throw new InvalidOperationException("Failed to get file name.");

            var contentStream = await response.Content.ReadAsStreamAsync();

            return (fileName, contentStream);
        }

        public async Task<VadapavSearchResults> SearchAsync(string searchTerm)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(searchTerm);

            var requestUri = EndPointProvider.Create(
                EndPointProvider.Search,
                searchTerm);

            var response = await _client.GetFromJsonAsync<VadapavSearchResponse>(requestUri) ??
                throw new InvalidOperationException($"Failed to get search results for search term '{searchTerm}'.");

            return response.Data.WrapAsSearchResults();
        }

        public async Task<List<VadapavFile>> SearchFilesAsync(string searchTerm)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(searchTerm);

            var results = await SearchAsync(searchTerm);

            return results.Files;
        }

        public async Task<List<VadapavDirectory>> SearchDirectoriesAsync(string searchTerm)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(searchTerm);

            var results = await SearchAsync(searchTerm);

            return results.Directories;
        }
    }
}
