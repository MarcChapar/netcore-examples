using System;

namespace Helpers.ParameterBinding.FromQuery.OnlyEnum
{
    public class OnlyEnumAttribute : Attribute, IOnlyEnumAttribute
    {
        public OnlyEnumAttribute(Type enumType)
        {
            this.EnumType = enumType;
        }

        public Type EnumType { get; set; }

        //public bool ParseToEnum { get; set; }
    }
}
