using System;
using System.Linq;
using System.Threading.Tasks;
using Helpers.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Helpers.ParameterBinding.FromQuery.OnlyEnum
{
    public class OnlyEnumModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult =
                bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            System.Reflection.PropertyInfo propInfo = bindingContext.ModelMetadata.ContainerType.GetProperty(bindingContext.ModelMetadata.PropertyName);
            if (propInfo == null)
            {
                return Task.CompletedTask;
            }

            var attribute = propInfo.GetCustomAttributes(typeof(OnlyEnumAttribute), false).FirstOrDefault() as OnlyEnumAttribute;
            if (attribute == null)
            {
                return Task.CompletedTask;
            }

            if (!EnumValidator.Validate(attribute.EnumType, valueProviderResult.ToString(), out object parsedEnum))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, string.Format("Object \"{0}\" of type {1} is not a {2} type", valueProviderResult.ToString(), "String", attribute.EnumType.Name));
            }
            else
            {
                if (propInfo.PropertyType == typeof(string))
                {
                    bindingContext.Result = ModelBindingResult.Success(parsedEnum.ToString());
                }
                else if (propInfo.PropertyType == typeof(int) || Nullable.GetUnderlyingType(propInfo.PropertyType) == typeof(int))
                {
                    bindingContext.Result = ModelBindingResult.Success((int)parsedEnum);
                }
                else if (propInfo.PropertyType == attribute.EnumType || Nullable.GetUnderlyingType(propInfo.PropertyType) == attribute.EnumType)
                {
                    bindingContext.Result = ModelBindingResult.Success(parsedEnum);
                }
                else
                {
                    bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, string.Format("Parameter of type {0} must be declared as an Integer, String or {1} type.", propInfo.PropertyType.Name, attribute.EnumType.Name));
                }
            }

            return Task.CompletedTask;
        }
    }
}
