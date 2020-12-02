using System;
using Newtonsoft.Json;

namespace Helpers.ParameterBinding.FromBody
{
    public class OnlyLongId : JsonConverter
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
                        long numericValue;
                        if (long.TryParse(reader.Value.ToString(), out numericValue))
                        {
                            if (numericValue > 0)
                            {
                                return defaultSerializer.Deserialize(reader, objectType);
                            }

                            throw new JsonSerializationException(string.Format("Object \"{0}\" of type {1} is not a positive value", reader.Value, reader.TokenType));
                        }

                        throw new JsonSerializationException(string.Format("Object \"{0}\" of type {1} is not a long type", reader.Value, reader.TokenType));
                    }

                case JsonToken.Integer:
                    {
                        if (long.Parse(reader.Value.ToString()) > 0)
                        {
                            return defaultSerializer.Deserialize(reader, objectType);
                        }

                        throw new JsonSerializationException(string.Format("Object \"{0}\" of type {1} is not a positive value", reader.Value, reader.TokenType));
                    }

                default:
                    throw new JsonSerializationException(string.Format("Object \"{0}\" of type {1} is not a long type", reader.Value, reader.TokenType));
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
