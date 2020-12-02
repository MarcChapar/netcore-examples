using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Helpers.ParameterBinding.FromQuery.OnlyBoolean
{
    public class OnlyBooleanModelBinder : IModelBinder
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

            bool isNumericValue = int.TryParse(valueProviderResult.ToString(), out _);
            if (isNumericValue)
            {
                if (valueProviderResult.ToString() == "1" || valueProviderResult.ToString() == "0")
                {
                    var result = valueProviderResult.ToString() == "1";
                    bindingContext.Result = ModelBindingResult.Success(result);
                }
                else
                {
                    bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, string.Format("Object \"{0}\" of type {1} is not a boolean type", valueProviderResult.ToString(), "Int"));
                }

                return Task.CompletedTask;
            }

            if (valueProviderResult.ToString().ToLower() == "true" || valueProviderResult.ToString().ToLower() == "false")
            {
                var result = valueProviderResult.ToString().ToLower() == "true";
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, string.Format("Object \"{0}\" of type {1} is not a boolean type", valueProviderResult.ToString(), "String"));
            }

            return Task.CompletedTask;
        }
    }
}
