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
public class AuthorizeOwnerAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _routeIdName;
    private readonly string _resourceType;

    public AuthorizeOwnerAttribute(string resourceType, string routeIdName = "id")
    {
        _resourceType = resourceType.ToLower(); 
        _routeIdName = routeIdName;
    }

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

    var idString = context.RouteData.Values[_routeIdName]?.ToString();
    if (!Guid.TryParse(idString, out Guid entityId))
    {
        context.Result = new BadRequestObjectResult("Invalid ID.");
        return;
    }

    if (roles != null && roles.Contains("ROLE_ADMIN")) return;

    bool isOwner = false;

    switch (_resourceType)
    {
        case "idea":
            var idea = await unitOfWork.Ideas.GetByIdAsync(entityId);
            isOwner = idea != null && idea.UserId.ToString() == userId;
            break;

        case "topic":
            var topic = await unitOfWork.Topics.GetByIdAsync(entityId);
            isOwner = topic != null && topic.UserId.ToString() == userId;
            break;

        case "vote":
            var vote = await unitOfWork.Votes.GetByUserAndIdeaAsync(Guid.Parse(userId), entityId);
            isOwner = vote != null && vote.UserId.ToString() == userId;
            break;

        default:
            context.Result = new BadRequestObjectResult("Unsupported resource type.");
            return;
    }

    if (!isOwner)
    {
        context.Result = new ObjectResult(new { message = $"You do not have permission to modify or delete this {_resourceType}." })
        {
            StatusCode = StatusCodes.Status403Forbidden
        };
    }
}

}

