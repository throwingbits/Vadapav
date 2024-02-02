using System.Net.Http.Headers;
using System.Net.Http.Json;

using Vadapav.Http;
using Vadapav.Models;
using Vadapav.Models.Http;

namespace Vadapav
{
    public partial class VadapavClient : IVadapavClient
    {
        private readonly HttpClient _client;
        
        /// <inheritdoc/>
        public IVadapavUriBuilder UriBuilder { get; private set; }

        public VadapavClient(string baseAddress, int maxRetryCount = 5)
            : this(new Uri(baseAddress), maxRetryCount)
        {
        }

        public VadapavClient(Uri baseAddress, int maxRetryCount = 5)
        {
            _client = new HttpClient(new HttpRetryMessageHandler(maxRetryCount))
            {
                BaseAddress = baseAddress
            };

            UriBuilder = new VadapavUriBuilder(baseAddress);
        }

        public VadapavClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentException(null, nameof(client));

            if (_client.BaseAddress == null)
                ArgumentNullException.ThrowIfNull(_client.BaseAddress);

            UriBuilder = new VadapavUriBuilder(_client.BaseAddress);
        }

        /// <inheritdoc/>
        public async Task<VadapavDirectory> GetRootDirectoryAsync()
        {
            var endpoint = UriBuilder.GetRootDirectoryUri();

            var response = await _client.GetFromJsonAsync<VadapavDirectoryResponse>(endpoint) ??
                throw new InvalidOperationException("Failed to get root directory.");

            return response.Data.AsDirectory();
        }

        /// <inheritdoc/>
        public Task<VadapavDirectory> GetDirectoryAsync(VadapavDirectory directory)
        {
            ArgumentNullException
                .ThrowIfNull(directory);

            return GetDirectoryAsync(directory.Id);
        }

        /// <inheritdoc/>
        public Task<VadapavDirectory> GetDirectoryAsync(Guid directoryId)
        {
            return GetDirectoryAsync(directoryId.ToString());
        }

        /// <inheritdoc/>
        public async Task<VadapavDirectory> GetDirectoryAsync(string directoryId)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(directoryId);

            var endpoint = UriBuilder.GetDirectoryUri(directoryId);

            var response = await _client.GetFromJsonAsync<VadapavDirectoryResponse>(endpoint) ??
                throw new InvalidOperationException($"Failed to get directory '{directoryId}'.");

            return response.Data.AsDirectory();
        }

        /// <inheritdoc/>
        public Task<(string Name, Stream ContentStream)> GetFileAsync(VadapavFile file)
        {
            ArgumentNullException
                .ThrowIfNull(file);

            return GetFileAsync(file.Id);
        }

        /// <inheritdoc/>
        public Task<(string Name, Stream ContentStream)> GetFileAsync(Guid fileId)
        {
            return GetFileAsync(fileId.ToString());
        }

        /// <inheritdoc/>
        public async Task<(string Name, Stream ContentStream)> GetFileAsync(string fileId)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(fileId);

            var requestUri = UriBuilder.GetFileUri(fileId);

            var response = await _client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var fileName = GetFileNameFromResponse(response);
            var contentStream = await response.Content.ReadAsStreamAsync();

            return (fileName, contentStream);
        }

        /// <inheritdoc/>
        public Task<(string Name, Stream ContentStream)> GetFileRangeAsync(VadapavFile file, long from, long? to)
        {
            ArgumentNullException
                .ThrowIfNull(file);

            return GetFileRangeAsync(file.Id, from, to);
        }

        /// <inheritdoc/>
        public Task<(string Name, Stream ContentStream)> GetFileRangeAsync(Guid fileId, long from, long? to)
        {
            return GetFileRangeAsync(fileId.ToString(), from, to);
        }

        /// <inheritdoc/>
        public async Task<(string Name, Stream ContentStream)> GetFileRangeAsync(string fileId, long from, long? to)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(fileId);

            if (from > to)
                throw new InvalidOperationException($"The value of the parameter {nameof(from)} needs to be less than the value of the parameter {nameof(to)}.");

            var requestUri = UriBuilder.GetFileUri(fileId);

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Range = new RangeHeaderValue(from, to);

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var fileName = GetFileNameFromResponse(response);
            var contentStream = await response.Content.ReadAsStreamAsync();

            return (fileName, contentStream);
        }

        /// <inheritdoc/>
        public async Task<VadapavSearchResults> SearchAsync(string searchTerm)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(searchTerm);

            var requestUri = UriBuilder.GetSearchUri(searchTerm);

            var response = await _client.GetFromJsonAsync<VadapavSearchResponse>(requestUri) ??
                throw new InvalidOperationException($"Failed to get search results for search term '{searchTerm}'.");

            return response.Data.WrapAsSearchResults();
        }

        /// <inheritdoc/>
        public async Task<List<VadapavFile>> SearchFilesAsync(string searchTerm)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(searchTerm);

            var results = await SearchAsync(searchTerm);

            return results.Files;
        }

        /// <inheritdoc/>
        public async Task<List<VadapavDirectory>> SearchDirectoriesAsync(string searchTerm)
        {
            ArgumentException
                .ThrowIfNullOrWhiteSpace(searchTerm);

            var results = await SearchAsync(searchTerm);

            return results.Directories;
        }

        private string GetFileNameFromResponse(HttpResponseMessage response)
        {
            var contentDispositionHeader = response.Content.Headers.ContentDisposition ?? 
                throw new InvalidOperationException("The server response did not contain the Content-Disposition header.");
            
            var fileName = contentDispositionHeader.FileNameStar;

            if (string.IsNullOrWhiteSpace(fileName))
                throw new InvalidOperationException("Failed to get file name from Content-Disposition header.");

            return fileName;
        }
    }
}
