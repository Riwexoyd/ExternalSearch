using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Games.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

namespace Riwexoyd.ExternalSearch.Games.Tests.Services
{
    public sealed class OneCOnlineExternalSearchProviderTests
    {
        [Fact]
        public async Task SearchAsync_MustReturnSomeCollection()
        {
            // Arrange
            OneCOnlineExternalSearchProvider externalSearchProvider = new OneCOnlineExternalSearchProvider();

            // Act
            IEnumerable<GameSearchResult>? games = await externalSearchProvider.SearchAsync(new GameSearchOptions
            {
                GameTitle = "Elden"
            }, System.Threading.CancellationToken.None);

            // Assert
            Assert.NotEmpty(games);
        }
    }
}
