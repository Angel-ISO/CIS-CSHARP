using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CisAPI.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CisAPI.middlewares;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]

public class AuthorizeIdeaOwnerAttribute : Attribute, IAsyncAuthorizationFilter
{
       public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userContextService = context.HttpContext.RequestServices.GetService<UserContextService>();
        var unitOfWork = context.HttpContext.RequestServices.GetService<IUnitOfWork>();

        if (userContextService == null || unitOfWork == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userId = userContextService.GetUserId();
        var roles = userContextService.GetRole();

        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var ideaId = context.RouteData.Values["id"]?.ToString();
        if (!Guid.TryParse(ideaId, out Guid parsedIdeaId))
        {
            context.Result = new BadRequestObjectResult("Invalid idea ID.");
            return;
        }

        var idea = await unitOfWork.Ideas.GetByIdAsync(parsedIdeaId);
        if (idea == null)
        {
            context.Result = new NotFoundResult();
            return;
        }

        if (roles != null && roles.Contains("ROLE_ADMIN"))
            {
                return; 
            }

        if (idea.UserId.ToString() != userId)
        {
            context.Result = new ObjectResult(new { message = "You do not have permission to modify or delete ideas from other users." })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }
    }
}
