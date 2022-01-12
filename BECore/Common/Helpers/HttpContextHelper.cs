using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

using System;
using System.Collections.Generic;
using System.Linq;
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

        public static string[] GetStore(this HttpContext httpContext)
        {
            var cuaHangIds = httpContext?.User?.FindFirst(c => c.Type == Constants.Principal.Store)?.Value;
            if (!string.IsNullOrWhiteSpace(cuaHangIds))
            {
                return cuaHangIds.ToLower().Split(",");
            }
            return null;
        }

    }
}
