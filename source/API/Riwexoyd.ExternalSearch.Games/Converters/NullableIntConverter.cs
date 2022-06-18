using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Riwexoyd.ExternalSearch.Games.Converters
{
    internal sealed class NullableIntConverter : JsonConverter<int?>
    {
        private static readonly Regex DigitalRegex = new(@"\d+");

        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string stringValue = reader.GetString()!;
                Match? match = DigitalRegex.Match(stringValue);
                if (!match.Success)
                    return null;

                if (int.TryParse(match.Value, out int value))
                    return value;

                return null;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                int result = reader.GetInt32();
                return result;
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
