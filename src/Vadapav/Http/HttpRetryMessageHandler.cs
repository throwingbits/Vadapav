using System.Net;

namespace Vadapav.Http
{
    internal sealed class HttpRetryMessageHandler : DelegatingHandler
    {
        private readonly int _maxRetryCount;

        public HttpRetryMessageHandler(int maxRetryCount)
            : base(new HttpClientHandler())
        {
            _maxRetryCount = maxRetryCount;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            for (var i = 1; i <= _maxRetryCount; i++)
            {
                HttpResponseMessage? result = null;
                var delay = TimeSpan.Zero;

                try
                {
                    result = await base
                        .SendAsync(request, cancellationToken)
                        .ConfigureAwait(false);

                    if (!IsLastAttempt(i) && ((int)result.StatusCode >= 500 || result.StatusCode is HttpStatusCode.RequestTimeout or HttpStatusCode.TooManyRequests))
                    {
                        delay = result.Headers.RetryAfter switch
                        {
                            { Date: { } date } => date - DateTimeOffset.UtcNow,
                            { Delta: { } delta } => delta,
                            _ => TimeSpan.Zero,
                        };

                        result.Dispose();
                    }
                    else
                    {
                        return result;
                    }
                }
                catch (HttpRequestException)
                {
                    result?.Dispose();

                    if (IsLastAttempt(i))
                        throw;
                }
                catch (TaskCanceledException ex) when (ex.CancellationToken != cancellationToken)
                {
                    result?.Dispose();

                    if (IsLastAttempt(i))
                        throw;
                }

                await Task
                    .Delay(delay, cancellationToken)
                    .ConfigureAwait(false);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private bool IsLastAttempt(int i) => i >= _maxRetryCount;
    }
}
