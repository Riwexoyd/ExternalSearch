using Riwexoyd.ExternalSearch.Games.Contracts;
using Riwexoyd.ExternalSearch.Games.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

namespace Riwexoyd.ExternalSearch.Games.Tests.Services
{
    public sealed class InteresExternalSearchProviderTests
    {
        [Fact]
        public async Task SearchAsync_MustReturnSomeCollection()
        {
            // Arrange
            InteresExternalSearchProvider externalSearchProvider = new InteresExternalSearchProvider();

            // Act
            IEnumerable<GameSearchResult>? games = await externalSearchProvider.SearchAsync(new GameSearchOptions
            {
                GameTitle = "Elden"
            }, System.Threading.CancellationToken.None);

            // Assert
            Assert.NotEmpty(games);
        }

        [Fact]
        public async Task SearchAsync_MustReturnOnlyPC()
        {
            // Arrange
            InteresExternalSearchProvider externalSearchProvider = new InteresExternalSearchProvider();

            // Act
            IEnumerable<GameSearchResult>? games = await externalSearchProvider.SearchAsync(new GameSearchOptions
            {
                GameTitle = "Guardians of galaxy"
            }, System.Threading.CancellationToken.None);

            // Assert
            Assert.DoesNotContain(games, result => result.GameTitle.Contains("Xbox") || result.GameTitle.Contains("PS4") || result.GameTitle.Contains("PS5"));
        }
    }
}
