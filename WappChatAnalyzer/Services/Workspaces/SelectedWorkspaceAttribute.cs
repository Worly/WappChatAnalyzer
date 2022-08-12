using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WappChatAnalyzer.Auth;

namespace WappChatAnalyzer.Services.Workspaces
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SelectedWorkspaceAttribute : Attribute, IResourceFilter
    {
        public SelectedWorkspaceAttribute()
        {

        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!context.HttpContext.Items.ContainsKey("WorkspaceId"))
            {
                context.Result = new JsonResult(new { message = "Missing workspaceId" }) { StatusCode = StatusCodes.Status400BadRequest };
                return;
            }

            var workspaceService = context.HttpContext.RequestServices.GetService<IWorkspaceService>();
            var user = context.HttpContext.CurrentUser();

            if (workspaceService.GetById(user.Id, (int)context.HttpContext.Items["WorkspaceId"], includeShared: user.VerifiedEmail) == null)
            {
                context.Result = new JsonResult(new { message = "Wrong workspaceId" }) { StatusCode = StatusCodes.Status404NotFound };
                return;
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            
        }
    }
}
