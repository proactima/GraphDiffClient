using System;
using Newtonsoft.Json;

namespace GraphDiffClient
{
    public class GraphUserConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            reader.Read();

            return new GraphResponse();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (GraphResponse).IsAssignableFrom(objectType);
        }
    }
}