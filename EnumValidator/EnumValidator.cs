using System;

namespace EnumValidator
{
    public static class EnumValidator
    {
        public static bool Validate<T>(string value, out object result)
        {
            bool valid = false;
            value = NormalizerEnum.NormalizeStr(value);

            if (Enum.TryParse(typeof(T), value, true, out var parsedValue))
            {
                valid = Enum.IsDefined(typeof(T), parsedValue);
            }

            result = parsedValue;
            return valid;
        }

        public static bool Validate(Type type, string value, out object result)
        {
            bool valid = false;

            value = NormalizerEnum.NormalizeStr(value);

            if (Enum.TryParse(type, value, true, out var parsedValue))
            {
                valid = Enum.IsDefined(type, parsedValue);
            }

            result = parsedValue;
            return valid;
        }
    }
}
