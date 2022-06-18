using Riwexoyd.ExternalSearch.Games.Converters;

using System.Text.Json.Serialization;

namespace Riwexoyd.ExternalSearch.Games.Models
{
    internal sealed class ZakaSearchResult
    {
        public string? Name { get; set; }

        [JsonConverter(typeof(PriceConverter))]
        public int? Price { get; set; }

        public string? Url { get; set; }
    }
}
