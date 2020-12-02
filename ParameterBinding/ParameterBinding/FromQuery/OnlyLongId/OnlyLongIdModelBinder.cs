using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Helpers.ParameterBinding.FromQuery.OnlyLongId
{
    public class OnlyLongIdModelBinder : IModelBinder
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

            long realValue;
            bool isNumericValue = long.TryParse(valueProviderResult.ToString(), out realValue);

            if (isNumericValue)
            {
                if (realValue > 0)
                {
                    bindingContext.Result = ModelBindingResult.Success(realValue);
                }
                else
                {
                    bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, string.Format("Object \"{0}\" of type {1} is not a positive value", valueProviderResult.ToString(), "Int"));
                }

                return Task.CompletedTask;
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, string.Format("Object \"{0}\" of type {1} is not an numeric value", valueProviderResult.ToString(), "string"));

                return Task.CompletedTask;
            }
        }
    }
}
