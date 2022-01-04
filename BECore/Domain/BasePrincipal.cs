using Common.Helpers;
using Microsoft.AspNetCore.Http;
using System;

namespace Domain
{
    public class BasePrincipal : BaseResult
    {
        protected Guid _userId;
        protected string _permission;
        protected string _hoTen;
        protected string _userName;

        public BasePrincipal(IHttpContextAccessor httpContextAccessor)
        {
            _userId = httpContextAccessor.HttpContext.GetUserId();
            _permission = httpContextAccessor.HttpContext.GetPermission();
            _hoTen = httpContextAccessor.HttpContext.GetFullName();
            _userName = httpContextAccessor.HttpContext.GetUserName();
        }
    }
}
