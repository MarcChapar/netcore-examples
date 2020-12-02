using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Helpers.ActionFilters
{
    public class ValidModelFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var exceptions = new List<string>();

                foreach (var state in context.ModelState)
                {
                    if (state.Value.Errors.Count != 0)
                    {
                        exceptions.AddRange(state.Value.Errors.Select(error => "Invalid parameter '" + state.Key + "': " + error.ErrorMessage));
                    }
                }

                context.Result = new BadRequestObjectResult(string.Join(',', exceptions));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
