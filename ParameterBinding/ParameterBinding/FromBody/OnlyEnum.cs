using System;
using System.IO;
using Helpers.Enums;
using Newtonsoft.Json;

namespace Helpers.ParameterBinding.FromBody
{
    public class OnlyEnum : JsonConverter
    {
        private readonly JsonSerializer defaultSerializer = new JsonSerializer();

        private Type conversionType;

        public OnlyEnum(Type conversionType)
        {
            this.conversionType = conversionType;
        }

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
            if (reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Integer)
            {
                if (objectType == typeof(string))
                {
                    if (EnumValidator.Validate(conversionType, reader.Value.ToString(), out object parsedEnum))
                    {
                        var newReader = CreateJsonReader("'" + parsedEnum.ToString() + "'");
                        return defaultSerializer.Deserialize(newReader, objectType);
                    }
                }
                else if (objectType == typeof(int) || Nullable.GetUnderlyingType(objectType) == typeof(int))
                {
                    if (EnumValidator.Validate(conversionType, reader.Value.ToString(), out object parsedEnum))
                    {
                        var newReader = CreateJsonReader(((int)parsedEnum).ToString());
                        return defaultSerializer.Deserialize(newReader, objectType);
                    }
                }
                else if (objectType == this.conversionType || Nullable.GetUnderlyingType(objectType) == this.conversionType)
                {
                    if (EnumValidator.Validate(conversionType, reader.Value.ToString(), out object parsedEnum))
                    {
                        var newReader = CreateJsonReader(((int)parsedEnum).ToString());
                        return defaultSerializer.Deserialize(newReader, objectType);
                    }
                }
                else
                {
                    throw new JsonSerializationException(string.Format("Parameter of type {0} must be declared as an Integer, String or {1} type.", objectType.Name, conversionType.Name));
                }
            }

            throw new JsonSerializationException(string.Format("Object \"{0}\" is not a valid {1} value.", reader.Value, conversionType.Name));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private JsonTextReader CreateJsonReader(string json)
        {
            var jsonReader = new JsonTextReader(new StringReader(json));
            while (jsonReader.TokenType == JsonToken.None)
            {
                if (!jsonReader.Read())
                {
                    break;
                }
            }

            return jsonReader;
        }
    }
}
