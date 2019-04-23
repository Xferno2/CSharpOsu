using CSharpOsu.Util.Enums;
using Newtonsoft.Json;
using System;

namespace CSharpOsu.Util.Converters
{
    // Thanks to Game4all and his circles.NET project (https://github.com/Game4all/circles.NET)
    public class ApprovedConvert : JsonConverter
    {
        public ApprovedConvert()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => (ApprovedStatus)int.Parse((string)reader.Value);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}