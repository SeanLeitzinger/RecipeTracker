using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using RecipeTracker.Api.ApiMessages;
using RecipeTracker.Api.Requests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeTracker.Api.Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Guid.TryParse(context.HttpContext.User.FindFirst("sub")?.Value, out Guid callerId);

            if (callerId == Guid.Empty && !context.Filters.Any(m => m.GetType() == typeof(AllowAnonymousFilter)))
            {
                context.Result = new BadRequestObjectResult(ValidationMessages.CallerIdRequired);

                return base.OnActionExecutionAsync(context, next);
            }

            var modelState = context.ModelState;

            if (!modelState.IsValid)
            {
                var errors = modelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage).ToList();
                context.Result = new BadRequestObjectResult(errors);

                return base.OnActionExecutionAsync(context, next);
            }

            var key = string.Empty;
            object value = null;

            foreach (var item in context.ActionArguments)
            {
                if (typeof(Request).IsAssignableFrom(item.Value.GetType()))
                {
                    key = item.Key;
                    value = item.Value;

                    break;
                }
            }

            if (value == null)
                context.Result = new BadRequestObjectResult(ValidationMessages.NotATypeOfRequest);

            var request = value as Request;

            request.CallerId = callerId;

            return base.OnActionExecutionAsync(context, next);
        }
    }
}
