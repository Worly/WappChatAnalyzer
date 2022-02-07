using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;

namespace WappChatAnalyzer.Auth
{
    public static class AuthExtensions
    {
        public static User CurrentUser(this HttpContext httpContext)
        {
            return httpContext.Items["User"] as User;
        }
    }
}
