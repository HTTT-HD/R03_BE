using Common.Utils;
using Common.ViewModels.Store;

using System;
using System.Threading.Tasks;

namespace Application.StoreServices
{
    public interface IStoreService
    {
        Task<ServiceResponse> AddOrUpdate(StoreViewModel model);
        Task<ServiceResponse> GetById(Guid id);
        Task<ServiceResponse> Delete(Guid id);
        Task<ServiceResponse> GetAll(StoreRequest request);
        Task<ServiceResponse> GetAllForUser(StoreRequest request);
    }
}
