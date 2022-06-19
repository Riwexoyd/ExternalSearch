using Riwexoyd.ExternalSearch.Games.Contracts;

namespace Riwexoyd.ExternalSearch.Games.Services
{
    internal abstract class HttpGameExternalSearchProvider : GameExternalSearchProvider
    {
        public static readonly HttpClient HttpClient = new HttpClient();

        public override async Task<IEnumerable<GameSearchResult>> SearchAsync(GameSearchOptions options, CancellationToken cancellationToken)
        {
            string uri = GetSearchUri(options);
            HttpRequestMessage request = new(HttpMethod.Get, uri);
            request.Headers.Add("x-requested-with", "XMLHttpRequest");
            using HttpResponseMessage? httpResponse = await HttpClient.SendAsync(request, cancellationToken);

            if (!httpResponse.IsSuccessStatusCode)
                return Enumerable.Empty<GameSearchResult>();

            Stream stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            if (stream.Length == 0)
                return Enumerable.Empty<GameSearchResult>();

            IEnumerable<GameSearchResult> data = await GetDataFromStream(stream, cancellationToken);

            return data;
        }

        protected abstract Task<IEnumerable<GameSearchResult>> GetDataFromStream(Stream stream, CancellationToken cancellationToken);
    }
}
