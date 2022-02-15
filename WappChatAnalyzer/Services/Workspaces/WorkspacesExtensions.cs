using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WappChatAnalyzer.Services.Workspaces
{
    public static class WorkspacesExtensions
    {
        public static int SelectedWorkspace(this HttpContext httpContext)
        {
            return (int)httpContext.Items["WorkspaceId"];
        }
    }
}
