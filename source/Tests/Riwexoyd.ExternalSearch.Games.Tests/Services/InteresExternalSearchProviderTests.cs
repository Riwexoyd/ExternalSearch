﻿using Riwexoyd.ExternalSearch.Games.Contracts;
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

        [Theory]
        [InlineData("Guardians of galaxy")]
        [InlineData("Skyrim")]
        public async Task SearchAsync_MustReturnOnlyPC(string title)
        {
            // Arrange
            InteresExternalSearchProvider externalSearchProvider = new InteresExternalSearchProvider();

            // Act
            IEnumerable<GameSearchResult>? games = await externalSearchProvider.SearchAsync(new GameSearchOptions
            {
                GameTitle = title
            }, System.Threading.CancellationToken.None);

            // Assert
            Assert.DoesNotContain(games, result => result.GameTitle.Contains("Xbox") || 
                result.GameTitle.Contains("PS4") || 
                result.GameTitle.Contains("PS5") || 
                result.GameTitle.Contains("Xbox One") ||
                result.GameTitle.Contains("Switch"));
        }
    }
}
