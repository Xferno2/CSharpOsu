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
            var value = Convert.ToInt32(reader.Value);
            bool boolValue = value != 0;
            return boolValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}