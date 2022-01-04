
using Common.Utils;
using Common.ViewModels.Permission;

using System.Threading.Tasks;

namespace Application.PermissionServices
{
    public interface IPermissionService
    {
        Task<ServiceResponse> AddPermissionDefault();
        Task<ServiceResponse> GetAllPermisison(PermissionRequest request);
    }
}
