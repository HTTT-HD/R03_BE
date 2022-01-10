using Common.Helpers;
using Common.Utils;
using Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Application.FileServices
{
    public class UploadFileService : BaseResult, IUploadFileService
    {
        public async Task<ServiceResponse> UploadFileAsync(IFormFile file)
        {
            if (file == null) return Forbidden(Constants.CodeError.NotValue, Constants.MessageResponse.NotValue);


            string fileName = String.Format("{0}{1}", file.FileName.Split('.')[0], Path.GetExtension(file.FileName));
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), Constants.AuthConfig.FolderFileDefault, fileName);

            string fullPath2 = Path.Combine("/" + Constants.AuthConfig.FolderFileDefault, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok(fullPath2);
        }

        public async Task<ServiceResponse> UploadListFileAsync(List<IFormFile> files)
        {
            if (files == null || !files.Any())
                return Forbidden(Constants.CodeError.NotValue, Constants.MessageResponse.NotValue);
            var path = new List<string>();
            foreach (var file in files)
            {
                string fileName = String.Format("{0}{1}", file.FileName.Split('.')[0], Path.GetExtension(file.FileName));
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), Constants.AuthConfig.FolderFileDefault, fileName);
                string fullPath2 = Path.Combine("/" + Constants.AuthConfig.FolderFileDefault, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                path.Add(fullPath2);
            } 
            return Ok(path);
        }
    }
}
