using System;
using Newtonsoft.Json;

namespace Helpers.ParameterBinding.FromBody
{
    public class OnlyIntegerId : JsonConverter
    {
        private readonly JsonSerializer defaultSerializer = new JsonSerializer();

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                    {
                        int numericValue;
                        if (int.TryParse(reader.Value.ToString(), out numericValue))
                        {
                            if (numericValue > 0)
                            {
                                return defaultSerializer.Deserialize(reader, objectType);
                            }

                            throw new JsonSerializationException(string.Format("Object \"{0}\" of type {1} is not a positive value", reader.Value, reader.TokenType));
                        }

                        throw new JsonSerializationException(string.Format("Object \"{0}\" of type {1} is not an integer type", reader.Value, reader.TokenType));
                    }

                case JsonToken.Integer:
                    {
                        if (int.Parse(reader.Value.ToString()) > 0)
                        {
                            return defaultSerializer.Deserialize(reader, objectType);
                        }

                        throw new JsonSerializationException(string.Format("Object \"{0}\" of type {1} is not a positive value", reader.Value, reader.TokenType));
                    }

                default:
                    throw new JsonSerializationException(string.Format("Object \"{0}\" of type {1} is not an integer type", reader.Value, reader.TokenType));
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
