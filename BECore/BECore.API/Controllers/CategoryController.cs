using Application.CategoryServices;

using Common.Utils;
using Common.ViewModels.Category;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace BECore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("create-or-update")]
        public async Task<ServiceResponse> AddOrUpdate(CategoryViewModel model)
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

        [HttpGet("get-all")]
        public async Task<ServiceResponse> GetAll([FromQuery] CategoryRequest request)
        {
            return await _service.GetAll(request);
        }
    }
}
