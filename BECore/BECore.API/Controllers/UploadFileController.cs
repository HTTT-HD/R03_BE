using Application.FileServices;

using Common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BECore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UploadFileController : ControllerBase
    {
        private readonly IUploadFileService _service;

        public UploadFileController(IUploadFileService service)
        {
            _service = service;
        }

        [HttpPost("upload-file")]
        public async Task<ServiceResponse> UploadFileAsync(IFormFile file)
        {
            return await _service.UploadFileAsync(file);
        }

        [HttpPost("upload-list-file")]
        public async Task<ServiceResponse> UploadListFileAsync(List<IFormFile> files)
        {
            return await _service.UploadListFileAsync(files);
        }
    }
}
