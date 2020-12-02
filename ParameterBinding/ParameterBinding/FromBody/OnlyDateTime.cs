using System;
using Newtonsoft.Json;

namespace Helpers.ParameterBinding.FromBody
{
    public class OnlyDateTime : JsonConverter
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
            if (reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Date)
            {
                switch (objectType.Name)
                {
                    case "DateTimeOffset":
                        if (!DateTimeOffset.TryParse(reader.Value.ToString(), out DateTimeOffset dto))
                        {
                            throw new JsonSerializationException(string.Format("Object \"{0}\" of type {1} is not a DateTimeOffset type", reader.Value, reader.TokenType));
                        }
                        else if (reader.TokenType == JsonToken.String)
                        {
                            return dto;
                        }
                        else
                        {
                            return defaultSerializer.Deserialize(reader, objectType);
                        }

                    case "DateTime":
                        if (!DateTime.TryParse(reader.Value.ToString(), out DateTime dt))
                        {
                            throw new JsonSerializationException(string.Format("Object \"{0}\" of type {1} is not a DateTime type", reader.Value, reader.TokenType));
                        }
                        else if (reader.TokenType == JsonToken.String)
                        {
                            return dt;
                        }
                        else
                        {
                            return defaultSerializer.Deserialize(reader, objectType);
                        }

                    default:
                        throw new JsonSerializationException(string.Format("Object \"{0}\" of type {1} is not a DateTime or DateTimeOffset type", reader.Value, reader.TokenType));
                }
            }

            throw new JsonSerializationException(string.Format("Object \"{0}\" of type {1} is not a string property type", reader.Value, reader.TokenType));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
