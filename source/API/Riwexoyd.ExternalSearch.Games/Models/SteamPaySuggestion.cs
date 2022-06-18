namespace Riwexoyd.ExternalSearch.Games.Models
{
    internal sealed class SteamPaySuggestion
    {
        public string? Value { get; set; }

        public SteamPaySearchData Data { get; set; } = new SteamPaySearchData();
    }
}
