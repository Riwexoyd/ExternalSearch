using Riwexoyd.ExternalSearch.Games.Converters;

using System.Text.Json.Serialization;

namespace Riwexoyd.ExternalSearch.Games.Models
{
    internal sealed class SteamBuySearchResult
    {
        [JsonConverter(typeof(IntConverter))]
        public int All { get; set; }

        public string Html { get; set; } = string.Empty;

        public string Q { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}
