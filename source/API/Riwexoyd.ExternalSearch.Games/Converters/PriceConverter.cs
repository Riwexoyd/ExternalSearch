using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Riwexoyd.ExternalSearch.Games.Converters
{
    internal sealed class PriceConverter : JsonConverter<int?>
    {
        private static readonly Regex Regex = new Regex(@"\d+");

        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string stringValue = reader.GetString()!;
                Match? match = Regex.Match(stringValue);
                if (match.Success)
                    return int.Parse(match.Value);
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                int result = reader.GetInt32();
                return result;
            }

            throw new ArgumentException(nameof(reader));
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
