using System.Net.Http.Json;

using Vadapav.Http;
using Vadapav.Models;
using Vadapav.Models.Http;

namespace Vadapav
{
    public class VadapavClient : IVadapavClient
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Creates a new instance of the <see cref="VadapavClient"/> and supports configuration of the inner retry handler.
        /// </summary>
        /// <param name="maxRetryCount">How often requests should be retried before bubbling up a exception.</param>
        public VadapavClient(ushort maxRetryCount = 5)
        {
            _client = new HttpClient(new HttpRetryMessageHandler(maxRetryCount))
            {
                BaseAddress = new Uri("https://vadapav.mov")
            };
        }

        /// <summary>
        /// Creates a new instance of the <see cref="VadapavClient"/> and supports bringing your own <see cref="HttpClient"/> instance.
        /// </summary>
        /// <param name="client"></param>
        /// <exception cref="ArgumentException"></exception>
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

        public Task<Stream> GetFileAsync(VadapavFile file)
        {
            ArgumentNullException
                .ThrowIfNull(file);

            return GetFileAsync(file.Id);
        }

        public Task<Stream> GetFileAsync(Guid id)
        {
            return GetFileAsync(id.ToString());
        }

        public async Task<Stream> GetFileAsync(string id)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(id);

            var requestUri = EndPointProvider.Create(
                EndPointProvider.File,
                id);

            var response = await _client.GetStreamAsync(requestUri) ??
                throw new InvalidOperationException($"Failed to get file '{id}'.");

            return response;
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
