using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Helpers.ParameterBinding.FromQuery.OnlyDouble
{
    public class OnlyDoubleModelBinder : IModelBinder
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

            var valueToParse = valueProviderResult.ToString().Replace(',', '.');
            double realValue;
            bool isNumericValue = double.TryParse(valueToParse, NumberStyles.Any, CultureInfo.InvariantCulture, out realValue);

            if (isNumericValue)
            {
                if (realValue > 0)
                {
                    bindingContext.Result = ModelBindingResult.Success(realValue);
                }
                else
                {
                    bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, string.Format("Object \"{0}\" of type {1} is not a positive value", valueProviderResult.ToString(), "Double"));
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
