using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Helpers.ParameterBinding.FromQuery.OnlyDateTime
{
    public class OnlyDateTimeModelBinder : IModelBinder
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

            if (propInfo.PropertyType == typeof(DateTimeOffset) || Nullable.GetUnderlyingType(propInfo.PropertyType) == typeof(DateTimeOffset))
            {
                DateTimeOffset realValue;
                bool isDateTime = DateTimeOffset.TryParse(valueProviderResult.ToString(), out realValue);

                if (isDateTime)
                {
                    bindingContext.Result = ModelBindingResult.Success(realValue);
                }
                else
                {
                    bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, string.Format("Object \"{0}\" of type {1} is not a valid DateTimeOffset value", valueProviderResult.ToString(), "String"));
                }
            }
            else if (propInfo.PropertyType == typeof(DateTime) || Nullable.GetUnderlyingType(propInfo.PropertyType) == typeof(DateTime))
            {
                DateTime realValue;
                bool isDateTime = DateTime.TryParse(valueProviderResult.ToString(), out realValue);

                if (isDateTime)
                {
                    bindingContext.Result = ModelBindingResult.Success(realValue);
                }
                else
                {
                    bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, string.Format("Object \"{0}\" of type {1} is not a valid DateTime value", valueProviderResult.ToString(), "String"));
                }
            }
            else
            {
                    bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, string.Format("Object \"{0}\" of type {1} is not a valid DateTime or DateTimeOffset value", valueProviderResult.ToString(), "String"));
            }

            return Task.CompletedTask;
        }
    }
}
