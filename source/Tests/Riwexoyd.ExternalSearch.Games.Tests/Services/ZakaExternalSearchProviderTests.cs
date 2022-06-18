using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Games.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

namespace Riwexoyd.ExternalSearch.Games.Tests.Services
{
    public sealed class ZakaExternalSearchProviderTests
    {
        [Fact]
        public async Task SearchAsync_MustReturnSomeCollection()
        {
            // Arrange
            ZakaExternalSearchProvider zakaExternalSearchProvider = new ZakaExternalSearchProvider();

            // Act
            IEnumerable<GameSearchResult>? games = await zakaExternalSearchProvider.SearchAsync(new GameSearchOptions
            {
                GameTitle = "God of war"
            }, System.Threading.CancellationToken.None);

            // Assert
            Assert.NotEmpty(games);
        }
    }
}
