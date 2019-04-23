using Newtonsoft.Json;
using System;

namespace CSharpOsu.Util.Converters
{
    // Thanks to Game4all and his circles.NET project (https://github.com/Game4all/circles.NET)
    internal class BoolConvert : JsonConverter
    {
        public BoolConvert()
        {
        }

        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool.TryParse((string)reader.Value, out var bools);
            return bools;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}