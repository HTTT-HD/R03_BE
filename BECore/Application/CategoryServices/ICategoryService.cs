
using Common.Utils;
using Common.ViewModels.Category;

using System;
using System.Threading.Tasks;

namespace Application.CategoryServices
{
    public interface ICategoryService
    {
        Task<ServiceResponse> AddOrUpdate(CategoryViewModel model);
        Task<ServiceResponse> GetById(Guid id);
        Task<ServiceResponse> Delete(Guid id);
        Task<ServiceResponse> GetAll(CategoryRequest request);
    }
}
