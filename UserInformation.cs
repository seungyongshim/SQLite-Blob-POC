using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SQLite_Blob_POC
{
    public class UserInformation
    {
        public byte[] BLOB { get; set; }
        public string PASSWORD { get; set; }
        public string USER_GROUP { get; set; }
        public long USER_ID { get; set; }
        public string USER_NAME { get; set; }

        public override string ToString()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            options.Converters.Add(new ByteArrayConverter());

            return JsonSerializer.Serialize(this, options);
        }
    }

    public class ByteArrayConverter : JsonConverter<byte[]>
    {
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, byte[] values, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteStringValue(string.Join(" ", values.Select(v => string.Format("0x{0:X2}", v))));
            writer.WriteEndObject();
        }
    }
}