
using Common.Utils;
using Common.ViewModels.Product;

using System;
using System.Threading.Tasks;

namespace Application.ProductServices
{
    public interface IProductService
    {
        Task<ServiceResponse> AddOrUpdate(ProductViewModel model);
        Task<ServiceResponse> GetById(Guid id);
        Task<ServiceResponse> Delete(Guid id);
        Task<ServiceResponse> GetAll(ProductRequest request);
    }
}
