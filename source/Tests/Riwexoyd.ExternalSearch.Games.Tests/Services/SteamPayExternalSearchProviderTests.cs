using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Games.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

namespace Riwexoyd.ExternalSearch.Games.Tests.Services
{
    public sealed class SteamPayExternalSearchProviderTests
    {
        [Fact]
        public async Task SearchAsync_MustReturnSomeCollection()
        {
            // Arrange
            SteamPayExternalSearchProvider externalSearchProvider = new SteamPayExternalSearchProvider();

            // Act
            IEnumerable<GameSearchResult>? games = await externalSearchProvider.SearchAsync(new GameSearchOptions
            {
                GameTitle = "God of war"
            }, System.Threading.CancellationToken.None);

            // Assert
            Assert.NotEmpty(games);
        }
    }
}
