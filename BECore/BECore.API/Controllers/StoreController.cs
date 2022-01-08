using Application.StoreServices;
using Common.Utils;
using Common.ViewModels.Store;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BECore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _service;

        public StoreController(IStoreService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("create-or-update")]
        public async Task<ServiceResponse> AddOrUpdate(StoreViewModel model)
        {
            return await _service.AddOrUpdate(model);
        }

        [HttpGet("get")]
        public async Task<ServiceResponse> GetById(Guid id)
        {
            return await _service.GetById(id);
        } 

        [Authorize]
        [HttpDelete("delete")]
        public async Task<ServiceResponse> Delete(Guid id)
        {
            return await _service.Delete(id);
        }

        [HttpPost("get-all")]
        public async Task<ServiceResponse> GetAll(StoreRequest request)
        {
            return await _service.GetAll(request);
        }
    }
}
