using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.Services.Workspaces
{
    public class WorkspaceMiddleware
    {
        private readonly RequestDelegate _next;

        public WorkspaceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (int.TryParse(context.Request.Headers["workspace-id"].FirstOrDefault(), out int workspaceId))
                context.Items["WorkspaceId"] = workspaceId;

            await _next(context);
        }
    }
}
