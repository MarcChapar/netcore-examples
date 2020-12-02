using System.Linq;
using Helpers.ParameterBinding.FromQuery.OnlyBoolean;
using Helpers.ParameterBinding.FromQuery.OnlyDateTime;
using Helpers.ParameterBinding.FromQuery.OnlyDouble;
using Helpers.ParameterBinding.FromQuery.OnlyEnum;
using Helpers.ParameterBinding.FromQuery.OnlyIntegerId;
using Helpers.ParameterBinding.FromQuery.OnlyLongId;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Helpers.ParameterBinding.FromQuery
{
    public class CustomModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.IsComplexType)
            {
                return null;
            }

            var propName = context.Metadata.PropertyName;
            if (propName == null)
            {
                return null;
            }

            var propInfo = context.Metadata.ContainerType.GetProperty(propName);
            if (propInfo == null)
            {
                return null;
            }

            var attribute = propInfo.GetCustomAttributes(
                typeof(IOnlyBooleanAttribute), false).FirstOrDefault();
            if (attribute != null)
            {
                return new BinderTypeModelBinder(typeof(OnlyBooleanModelBinder));
            }

            attribute = propInfo.GetCustomAttributes(
                typeof(IOnlyIntegerIdAttribute), false).FirstOrDefault();
            if (attribute != null)
            {
                return new BinderTypeModelBinder(typeof(OnlyIntegerIdModelBinder));
            }

            attribute = propInfo.GetCustomAttributes(
                typeof(IOnlyEnumAttribute), false).FirstOrDefault();
            if (attribute != null)
            {
                return new BinderTypeModelBinder(typeof(OnlyEnumModelBinder));
            }

            attribute = propInfo.GetCustomAttributes(
                typeof(IOnlyDateTimeAttribute), false).FirstOrDefault();
            if (attribute != null)
            {
                return new BinderTypeModelBinder(typeof(OnlyDateTimeModelBinder));
            }

            attribute = propInfo.GetCustomAttributes(
                typeof(IOnlyLongIdAttribute), false).FirstOrDefault();
            if (attribute != null)
            {
                return new BinderTypeModelBinder(typeof(OnlyLongIdModelBinder));
            }

            attribute = propInfo.GetCustomAttributes(
                typeof(IOnlyDoubleAttribute), false).FirstOrDefault();
            if (attribute != null)
            {
                return new BinderTypeModelBinder(typeof(OnlyDoubleModelBinder));
            }

            return null;
        }
    }
}
