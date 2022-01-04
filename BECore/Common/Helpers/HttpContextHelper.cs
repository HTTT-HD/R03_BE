using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Common.Helpers
{
    public static class HttpContextHelper
{
        public static Guid GetUserId(this HttpContext httpContext)
        {
            var currentUserId = httpContext?.User?.FindFirst(x => x.Type == Constants.Principal.UserId)?.Value;
            Guid.TryParse(currentUserId, out Guid userId);
            return userId;
        }

        public static string GetPermission(this HttpContext httpContext)
        {
            return httpContext?.User?.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
        }

        public static string GetFullName(this HttpContext httpContext)
        {
            return httpContext?.User?.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
        }

        public static string GetUserName(this HttpContext httpContext)
        {
            return httpContext?.User?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }

    }
}
