using Application.RoleServices;
using Common.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Common.ViewModels.Role;
using Common.ViewModels.Permission;
using Common.ViewModels.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace BECore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service=service;
        }
         
        [HttpPost("create-or-update")]
        public async Task<ServiceResponse> Create(RoleViewModel model)
        {
            return await _service.AddOrUpdate(model);
        }

        [HttpPost("get-all")]
        public async Task<ServiceResponse> GetAll(RoleRequest model)
        {
            return await _service.GetAll(model);
        }

        [HttpGet("get-by-id")]
        public async Task<ServiceResponse> GetById(Guid id)
        {
            return await _service.GetById(id);
        }

        [HttpDelete("delete")]
        public async Task<ServiceResponse> DeleteById(Guid id)
        {
            return await _service.DeleteAsnyc(id);
        }

        [HttpPost("add-permission-to-role")]
        public async Task<ServiceResponse> AddPermission(AddPermisisonToRole request)
        {
            return await _service.AddPermission(request);
        }

        [HttpPost("add-user-to-role")]
        public async Task<ServiceResponse> AddUser(AddUserToRole request)
        {
            return await _service.AddUser(request);
        }

        [HttpGet("get-permission-in-role")]
        public async Task<ServiceResponse> GetPermissionByRole([FromQuery]PermissionRequest request)
        {
            return await _service.GetPermissionInRole(request);
        }

        [HttpPost("get-user-in-role")]
        public async Task<ServiceResponse> GetUserInRole(UserInRole request)
        {
            return await _service.GetUserInRole(request);
        }

        [HttpPost("get-user-out-role")]
        public async Task<ServiceResponse> GetUserOutRole(UserInRole request)
        {
            return await _service.GetUserOutRole(request);
        }

    }
}
