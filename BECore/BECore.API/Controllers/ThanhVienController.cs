using Application.AuthenticationServices;
using Common.Utils;
using Common.ViewModels.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BECore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ThanhVienController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public ThanhVienController(IAuthenticationService service)
        {
            _service=service;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ServiceResponse> Create(IdentityViewModel model)
        {
            return await _service.CreateAsync(model);
        }

        [HttpPut("update")]
        public async Task<ServiceResponse> Update(IdentityUpdate model)
        {
            return await _service.UpdateAsync(model);
        }

        [HttpPost("get-all")]
        public async Task<ServiceResponse> GetAll(IdentityRequest request)
        {
            return await _service.GetAll(request);
        }

        [HttpGet("get-by-id")]
        public async Task<ServiceResponse> GetById(Guid id)
        {
            return await _service.GetById(id);
        }

        [HttpGet("user-login")]
        public async Task<ServiceResponse> GetUserLogin()
        {
            return await _service.GetUserLogin();
        }

        [HttpDelete("delete")]
        public async Task<ServiceResponse> DeleteById(Guid id)
        {
            return await _service.DeleteAsnyc(id);
        }

        [HttpPost("login")]
        public async Task<ServiceResponse> LoginAction(LoginViewModel model)
        {
            return await _service.LoginAction(model);
        }

        [HttpGet("index-user")]
        public async Task IndexUsers()
        {
            await _service.IndexUsers();
        }
    }
}
