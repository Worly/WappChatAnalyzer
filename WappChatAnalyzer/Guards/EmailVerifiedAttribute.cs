using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using WappChatAnalyzer.Auth;

namespace WappChatAnalyzer.Guards
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EmailVerifiedAttribute : Attribute, IResourceFilter
    {
        public EmailVerifiedAttribute()
        {

        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var user = context.HttpContext.CurrentUser();
            if (user == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            if (!user.VerifiedEmail)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Email not verified" }) { StatusCode = StatusCodes.Status403Forbidden };
                return;
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }
    }
}
