using Application.PermissionServices;
using Common.Utils;
using Common.ViewModels.Permission;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BECore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _service;

        public PermissionController(IPermissionService service)
        {
            _service=service;
        }

        [HttpPost("add-permission-default")]
        public async Task<ServiceResponse> Create()
        {
            return await _service.AddPermissionDefault();
        }

        [HttpGet("permission-default")]
        public async Task<ServiceResponse> GetAll([FromQuery] PermissionRequest request)
        {
            return await _service.GetAllPermisison(request);
        }
    }
}
