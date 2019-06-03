using Newtonsoft.Json;
using System;

namespace CSharpOsu.Util.Converters
{
    // Thanks to Game4all and his circles.NET project (https://github.com/Game4all/circles.NET)
    internal class BoolConvert : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try {
                bool boolValue = Convert.ToInt32(reader.Value) != 0;
                return boolValue;
            }
            catch (Exception) {
                throw new Exception("The response from the server was not a 0 or a 1." +
                    System.Environment.NewLine+
                    "Server response: " +reader.Value.ToString()
                    );
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}