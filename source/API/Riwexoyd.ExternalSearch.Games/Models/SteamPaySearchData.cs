using Riwexoyd.ExternalSearch.Games.Converters;

using System.Text.Json.Serialization;

namespace Riwexoyd.ExternalSearch.Games.Models
{
    internal sealed class SteamPaySearchData
    {
        public string? Link { get; set; }

        [JsonConverter(typeof(NullableIntConverter))]
        public int? Price { get; set; }

        public string? Value { get; set; }
    }
}
