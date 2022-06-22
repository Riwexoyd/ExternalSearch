using AngleSharp;
using AngleSharp.Dom;

using Riwexoyd.ExternalSearch.Games.Contracts;

namespace Riwexoyd.ExternalSearch.Games.Services
{
    internal abstract class ParseExternalSearchProvider : GameExternalSearchProvider
    {
        public override async Task<IEnumerable<GameSearchResult>> SearchAsync(GameSearchOptions options, CancellationToken cancellationToken)
        {
            string address = GetSearchUri(options);
            var config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext browsingContext = BrowsingContext.New(config);
            IDocument document = await browsingContext.OpenAsync(address, cancellationToken);

            return ParseDocument(document);
        }

        protected abstract IEnumerable<GameSearchResult> ParseDocument(IDocument document);
    }
}
