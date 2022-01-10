
using Common.Utils;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.FileServices
{
    public interface IUploadFileService
    {
        Task<ServiceResponse> UploadFileAsync(IFormFile file);
        Task<ServiceResponse> UploadListFileAsync(List<IFormFile> files);
    }
}
