using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Helpers.ParameterBinding.FromQuery.OnlyIntegerId
{
    public class OnlyIntegerIdModelBinder : IModelBinder
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

            int realValue;
            bool isNumericValue = int.TryParse(valueProviderResult.ToString(), out realValue);

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
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, string.Format("Object \"{0}\" of type {1} is not a numeric value", valueProviderResult.ToString(), "string"));

                return Task.CompletedTask;
            }
        }
    }
}
