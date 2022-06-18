namespace Riwexoyd.ExternalSearch.Games.Models
{
    internal sealed class SteamPaySearchResult
    {
        public string? Query { get; set; }

        public SteamPaySuggestion[]? Suggestions { get; set; }
    }
}
