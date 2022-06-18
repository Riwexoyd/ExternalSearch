using System.Text.Json;
using System.Text.Json.Serialization;

namespace Riwexoyd.ExternalSearch.Games.Converters
{
    internal sealed class IntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string data = reader.GetString()!;

            if (string.IsNullOrEmpty(data))
                return default;

            if (!int.TryParse(data, out int result))
                return default;
            
            return result;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
