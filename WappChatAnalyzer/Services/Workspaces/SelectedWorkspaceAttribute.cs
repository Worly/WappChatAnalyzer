using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

            if (!context.HttpContext.CurrentUser().Workspaces.Any(w => w.Id == (int)context.HttpContext.Items["WorkspaceId"]))
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
